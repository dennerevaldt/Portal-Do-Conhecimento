homeSite.controller('ContactController', function ($scope, $http) {
    $scope.resultMessage;
    $scope.formData; //formData is an object holding the name, email and message
    $scope.submitButtonDisabled = false;
    $scope.loading = false;
    $scope.submit = function (contactform) {
        $scope.submitButtonDisabled = true;
        if (contactform.$valid) {
            $scope.loading = true;
            $http({
                method: 'POST',
                url: '/site/contact',
                data: $.param($scope.formData),  //param method from jQuery
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
            }).success(function (data) {
                console.log(data);
                if (data.success) { //success comes from the return json object
                    $scope.submitButtonDisabled = true;
                    $scope.resultMessage = data.message;
                    $scope.result = 'bg-success';
                    $scope.loading = false;
                } else {
                    $scope.submitButtonDisabled = false;
                    $scope.resultMessage = data.message;
                    $scope.result = 'bg-danger';
                    $scope.loading = false;
                }
            });
        } else {
            $scope.submitButtonDisabled = false;
            $scope.resultMessage = 'Ops, estamos com problemas.. Tente novamente.';
            $scope.result = 'bg-danger';
            $scope.loading = false;
        }
    }
});

homeSite.controller('LoginController', function ($scope, $http) {
    $scope.resultMessage;
    $scope.submitButtonLogin = false;
    $scope.loading = false;

    $scope.submit = function (loginform) {
        $scope.submitButtonLogin = true;
        if (loginform.$valid) {
            $scope.loading = true;
            $http({
                method: 'POST',
                url: '/account/login',
                data: $.param($scope.formLogin),  //param method from jQuery
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
            }).success(function (data) {
                console.log(data);
                if (data.success) { //success comes from the return json object
                    $scope.submitButtonLogin = true;
                    $scope.resultMessage = data.message;
                    $scope.result = 'bg-success';
                    $scope.loading = false;

                    if (data.returnUrl != null) {
                        location.href = data.returnUrl;
                    }
                    else {
                        location.href = data.location;
                    }
                    
                } else {
                    $scope.submitButtonLogin = false;
                    $scope.resultMessage = data.message;
                    $scope.result = 'bg-danger';
                    $scope.loading = false;
                }
            });
        } else {
            $scope.submitButtonLogin = false;
            $scope.resultMessage = 'Campos obrigatórios. Preencha-os corretamente.';
            $scope.result = 'bg-danger';
            $scope.loading = false;
        }
    }
});