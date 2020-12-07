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

	stage('Build Images') {
	    steps {
		script {
		    def webapi = docker.build("luukvdm/lp-webapi", "-f ./src/WebApi/WebApi/Dockerfile ./")
		    docker.withRegistry('https://registry.hub.docker.com', 'docker-hub-credentials') {
			schemaapi.push("${env.BUILD_NUMBER}")
			schemaapi.push("latest")
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

