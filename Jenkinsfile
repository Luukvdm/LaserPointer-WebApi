pipeline {
    agent any
    stages {
        /* stage('test') {
            steps {
		script {
		    def test = docker.build("lp-tests", "-f ./dockerfiles/Dockerfile-Test ./")
		    test.inside {
			sh 'dotnet test'
		    }
		}
            }
        } */

	stage('build && SonarQube analysis') {
            steps {
                withSonarQubeEnv('My SonarQube Server') {
		    // To lazy to create my own image and this one looks pretty good
		    sh "docker pull nosinovacao/dotnet-sonar:latest"
		    withSonarQubeEnv('sonarqube') {
			sh '''docker run --rm -v ${workspace}:/source nosinovacao/dotnet-sonar:latest \
			bash -c 'cd /source \
			&& dotnet /sonar-sacnner/SonarScanner.MSBuild.dll begin /k:${SONAR_AUTH_TOKEN} /version:buildVersion \
			&& dotnet restore \
			&& dotnet build -c Release \
			&& dotnet /sonar-scanner/SonarScanner.MSBuild.dll end'''
		    }
                }
            }
        }
        stage("Quality Gate") {
            steps {
                timeout(time: 1, unit: 'HOURS') {
                    // Parameter indicates whether to set pipeline to UNSTABLE if Quality Gate fails
                    // true = set pipeline to UNSTABLE, false = don't
                    waitForQualityGate abortPipeline: true
                }
            }
        }

	stage('Build Images') {
	    steps {
		script {
		    def webapi = docker.build("luukvdm/lp-webapi", "-f ./src/WebApi/WebApi/Dockerfile ./")
		    docker.withRegistry('https://registry.hub.docker.com', 'docker-hub-credentials') {
			webapi.push("${env.BUILD_NUMBER}")
			webapi.push("latest")
		    }

		    def identityserver = docker.build("luukvdm/lp-identityserver", "-f ./src/IdentityServer/Dockerfile ./")
		    docker.withRegistry('https://registry.hub.docker.com', 'docker-hub-credentials') {
			identityserver.push("${env.BUILD_NUMBER}")
			identityserver.push("latest")
		    }
		}

	    }
	}

    }
}

