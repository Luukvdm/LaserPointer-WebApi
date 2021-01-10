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
                withSonarQubeEnv('MySonarqubeServer') {
		    // To lazy to create my own image and this one looks pretty good
		    sh "docker pull nosinovacao/dotnet-sonar:latest"
		    sh '''docker run --rm \
			-v ${WORKSPACE}:/source \
			--network cd_lp_network \
			nosinovacao/dotnet-sonar:latest \
			bash -c "cd source \
			ls -la \
			&& dotnet /sonar-scanner/SonarScanner.MSBuild.dll begin /k:'laserpointer-webapi' /version:buildVersion \
			/d:sonar.host.url='${SONAR_HOST_URL}' /d:sonar.login='${SONAR_AUTH_TOKEN}' \
			&& dotnet restore \
			&& dotnet build -c Release \
			&& dotnet /sonar-scanner/SonarScanner.MSBuild.dll end /d:sonar.login='${SONAR_AUTH_TOKEN}'"'''
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

