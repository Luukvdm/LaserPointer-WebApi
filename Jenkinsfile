pipeline {
    agent any
    stages {
        stage('build') {
            steps {
            }
        }

        stage('test') {
            steps {
		docker.image('mcr.microsoft.com/dotnet/core/sdk:3.1').inside {
		    // git 'https://github.com/luukvdm/LaserPointer-WebApi'
		    sh 'dotnet test'
		}
            }
        }

	stage('build Backend image') {
	    steps {
		script {
		    def webapi = docker.build("luukvdm/lp-webapi", "-f src/WebApi/Dockerfile src/WebApi")
		    docker.withRegistry('https://registry.hub.docker.com', 'docker-hub-credentials') {
			schemaapi.push("${env.BUILD_NUMBER}")
			schemaapi.push("latest")
		    }

		    def identityserver = docker.build("luukvdm/lp-identityserver", "-f ./src/IdentityServer/Dockerfile src/IdentityServer")
		    docker.withRegistry('https://registry.hub.docker.com', 'docker-hub-credentials') {
			identityserver.push("${env.BUILD_NUMBER}")
			identityserver.push("latest")
		    }
		}

	    }
	}

    }
}

