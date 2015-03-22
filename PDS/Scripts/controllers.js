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

homeSite.controller('ChangeKeyController', function ($scope, $http) {
    $scope.submitButton = false;
    $scope.loading = false;

    $scope.submit = function (formChangeKey) {
        $scope.submitButton = true;
        if (formChangeKey.$valid) {
            $scope.loading = true;
            $http({
                method: 'POST',
                url: '/account/changekey',
                data: $.param($scope.Account),  //param method from jQuery
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
            }).success(function (data) {
                console.log(data);
                if (data.success) { //success comes from the return json object
                    $scope.submitButton = true;
                    $scope.resultMessage = data.message;
                    $scope.result = 'bg-success';
                    $scope.loading = false;
                } else {
                    $scope.submitButton = false;
                    $scope.resultMessage = data.message;
                    $scope.result = 'bg-danger';
                    $scope.loading = false;
                }
            });
        } else {
            $scope.submitButton = false;
            $scope.resultMessage = 'Campos obrigatórios. Preencha-os corretamente.';
            $scope.result = 'bg-danger';
            $scope.loading = false;
        }
    }

});

accountModule.controller('ConfirmAccountController', function ($scope, $http) {
    $scope.submitConfirmButton = false;
    $scope.loading = false;

    $scope.submit = function (confirmAccount) {
        $scope.submitConfirmButton = true;
        if (confirmAccount.$valid) {
            $scope.loading = true;

            console.log($scope.Account);

            if ($scope.Account.accountType =='T')
            {
                $http({
                    method: 'POST',
                    url: '/account/confirmaccountteacher',
                    data: $.param($scope.Account),  //param method from jQuery
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
                }).success(function (data) {
                    console.log(data);
                    if (data.success) { //success comes from the return json object
                        $scope.submitConfirmButton = true;
                        $scope.loading = false;

                        if (data.returnUrl != null) {
                            location.href = data.returnUrl;
                        }
                        else {
                            location.href = data.location;
                        }

                    } else {
                        $scope.submitConfirmButton = false;
                        $scope.resultMessage = data.message;
                        $scope.result = 'bg-danger';
                        $scope.loading = false;
                    }
                });
            }
            else
            {
                $http({
                    method: 'POST',
                    url: '/account/confirmaccountstudent',
                    data: $.param($scope.Account),  //param method from jQuery
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
                }).success(function (data) {
                    console.log(data);
                    if (data.success) { //success comes from the return json object
                        $scope.submitConfirmButton = true;
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
                        $scope.submitConfirmButton = false;
                        $scope.resultMessage = data.message;
                        $scope.result = 'bg-danger';
                        $scope.loading = false;
                    }
                });
            }
            


        } else {
            $scope.submitConfirmButton = false;
            $scope.resultMessage = 'Campos obrigatórios. Preencha-os corretamente.';
            $scope.result = 'bg-danger';
            $scope.loading = false;
        }
    }
});

changeKeyModule.controller('ChangeKeyConfirmController', function ($scope, $http) {
    $scope.loading = false;

    $scope.submit = function (formChangeKeyConfirm) {
        $scope.submitButton = true;
        if (formChangeKeyConfirm.$valid) {
            $scope.loading = true;
            $http({
                method: 'POST',
                url: '/account/changekeyconfirmed',
                data: $.param($scope.Account),  //param method from jQuery
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
            }).success(function (data) {
                console.log(data);
                if (data.success) { //success comes from the return json object
                    $scope.resultMessage = data.message;
                    $scope.result = 'bg-success';
                    $scope.loading = false;

                } else {
                    $scope.resultMessage = data.message;
                    $scope.result = 'bg-danger';
                    $scope.loading = false;
                }
            });
        } else {
            $scope.resultMessage = 'Campos obrigatórios. Preencha-os corretamente.';
            $scope.result = 'bg-danger';
            $scope.loading = false;
        }
    }

});

manageModule.controller('ManageController', function ($scope, $http, $timeout) {

    $scope.changeData = true;
    $scope.changeKey = false;
    $scope.deleteAccount = false;
    $scope.changePhoto = false;
    $scope.cK = "";
    $scope.cDt = "active";
    $scope.dAc = "";
    $scope.cPh = "";
    $scope.resultShow = false;
;


    $scope.changeDt = function () {
        $scope.changeData = true;
        $scope.changeKey = false;
        $scope.deleteAccount = false;
        $scope.changePhoto = false;

        $scope.cK = "";
        $scope.cDt = "active";
        $scope.dAc = "";
        $scope.cPh = "";
    }

    $scope.changeK = function () {
        $scope.changeData = false;
        $scope.changeKey = true;
        $scope.deleteAccount = false;
        $scope.changePhoto = false;

        $scope.cK = "active";
        $scope.cDt = "";
        $scope.dAc = "";
        $scope.cPh = "";
    }

    $scope.deleteAc = function () {
        $scope.changeData = false;
        $scope.changeKey = false;
        $scope.deleteAccount = true;
        $scope.changePhoto = false;

        $scope.cK = "";
        $scope.cDt = "";
        $scope.dAc = "active";
        $scope.cPh = "";
    }

    $scope.changePh = function () {
        $scope.changeData = false;
        $scope.changeKey = false;
        $scope.deleteAccount = false;
        $scope.changePhoto = true;

        $scope.cK = "";
        $scope.cDt = "";
        $scope.dAc = "";
        $scope.cPh = "active";
    }

    //submit key

    $scope.submitCk = function (formChangeKey) {
        $scope.submitButton = true;
        if (formChangeKey.$valid) {
            $scope.loading = true;
            $http({
                method: 'POST',
                url: '/account/changekeyconfirmed',
                data: $.param($scope.Account),  //param method from jQuery
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
            }).success(function (data) {
                console.log(data);
                if (data.success) { //success comes from the return json object
                    $scope.submitButton = true;
                    $scope.resultMessage = data.message;
                    $scope.result = 'bg-success';
                    $scope.loading = false;
                    $scope.resultShow = true;
                    $timeout(function () {
                        $scope.resultShow = false;
                    }, 3000);

                } else {
                    $scope.submitButton = false;
                    $scope.resultMessage = data.message;
                    $scope.result = 'bg-danger';
                    $scope.loading = false;
                }
            });
        } else {
            $scope.submitButton = false;
            $scope.resultMessage = 'Campos obrigatórios. Preencha-os corretamente.';
            $scope.result = 'bg-danger';
            $scope.loading = false;
        }
    }

    //submit delete

    $scope.submitAc = function (formDeleteAccount) {
        $scope.submitButton = true;
        if (formDeleteAccount.$valid) {
            $scope.loading = true;
            $http({
                method: 'POST',
                url: '/account/deleteaccount',
                data: $.param($scope.Account),  //param method from jQuery
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
            }).success(function (data) {
                console.log(data);
                if (data.success) { //success comes from the return json object
                    $scope.submitButton = true;
                    $scope.loading = false;
                    location.href = data.location;
                } else {
                    $scope.submitButton = false;
                    $scope.resultMessage = data.message;
                    $scope.result = 'bg-danger';
                    $scope.loading = false;
                }
            });
        } else {
            $scope.submitButton = false;
            $scope.resultMessage = 'Estamos com problemas, tente novamente.';
            $scope.result = 'bg-danger';
            $scope.loading = false;
        }
    }

    
    //submit change data

    $scope.submitCd = function (formChangeData) {
        $scope.submitButton = true;
        if (formChangeData.$valid) {
            $scope.loading = true;
            $http({
                method: 'POST',
                url: '/account/changedata',
                data: $.param($scope.Account),  //param method from jQuery
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
            }).success(function (data) {
                console.log(data);
                if (data.success) { //success comes from the return json object
                    $scope.submitButton = true;
                    $scope.resultMessage = data.message;
                    $scope.result = 'bg-success';
                    $scope.loading = false;

                } else {
                    $scope.submitButton = false;
                    $scope.resultMessage = data.message;
                    $scope.result = 'bg-danger';
                    $scope.loading = false;
                }
            });
        } else {
            $scope.submitButton = false;
            $scope.resultMessage = 'Campos obrigatórios. Preencha-os corretamente.';
            $scope.result = 'bg-danger';
            $scope.loading = false;
        }
    }

})

homePortal.controller('InviteFriendsController', function ($scope, $http) {
    $scope.submitButton = false;
    $scope.loading = false;

    $scope.submit = function (inviteForm) {
        $scope.submitButton = true;
        if (inviteForm.$valid) {
            $scope.loading = true;
            $http({
                method: 'POST',
                url: '/home/invitefriend',
                data: $.param($scope.Account),  //param method from jQuery
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
            }).success(function (data) {
                console.log(data);
                if (data.success) { //success comes from the return json object
                    $scope.submitButton = true;
                    $scope.resultMessage = data.message;
                    $scope.result = 'bg-success';
                    $scope.loading = false;

                } else {
                    $scope.submitButton = false;
                    $scope.resultMessage = data.message;
                    $scope.result = 'bg-danger';
                    $scope.loading = false;
                }
            });
        } else {
            $scope.submitButton = false;
            $scope.resultMessage = 'Campos obrigatórios. Preencha-os corretamente.';
            $scope.result = 'bg-danger';
            $scope.loading = false;
        }
    }


})