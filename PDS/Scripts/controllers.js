﻿homePortal.run(function ($rootScope, $http, ngProgress) {
   
    $rootScope.getallclasses = function (id) {
        //set progress bar
        ngProgress.color('#4285f4');
        ngProgress.start();

        $http({
            method: 'GET',
            url: '/classes/getall?id=' + id,
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).success(function (data) {
            //set 50% progress
            ngProgress.set(50);

            if (data != null) {
                $rootScope.listClasses = data;

                $http({
                    method: 'GET',
                    url: '/disciplines/getname?idDisc='+id,
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
                }).success(function (data) {
                    ngProgress.complete();
                    if (data != null) {
                        $rootScope.nameClasse = data;
                    } else {
                        Console.log('erro.');
                    }
                });

            } else {
                Console.log('erro ou lista vazia.');
            }
        });
    }

    $rootScope.getDataOneClass = function () {
        //set progress bar
        ngProgress.color('#4285f4');
        ngProgress.start();

        var pIdClass = location.search.split('?id=')[1];
        var dataParam = {
            idClass: pIdClass
        }

        $http({
            method: 'POST',
            url: '/classes/getoneclass',
            data: $.param(dataParam),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).success(function (data) {
            //end progress
            ngProgress.complete();

            if (data != null) {
                $rootScope.classe = data;
                $rootScope.nameClass = data.name;
                $rootScope.classesStudents = data.classesStudents;
                $rootScope.classesPublicationTeachers = data.classesPublicationTeachers;
                $rootScope.discipline = data.discipline;
            } else {
                Console.log('erro.');
            }
        });
    }

    $rootScope.getPublicationsTeacherProf = function () {
        //set progress bar
        ngProgress.color('#4285f4');
        ngProgress.start();

        $http({
            method: 'POST',
            url: '/teachers/GetPublications',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).success(function (data) {
            //end progress bar
            ngProgress.complete();

            if (data != null) {
                $rootScope.publicationsTeachersProfile = data;
            } else {
                Console.log('erro.');
            }
        });
    }

    $rootScope.getMessages = function (idStudent) {

        dataParam = {
            id: idStudent
        }

        $http({
            method: 'POST',
            url: '/message/getMessagesClasses',
            data: $.param(dataParam),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).success(function (data) {
            console.log(data);
            if (data != 0) {
                $rootScope.listMessages = data;
            }
            else {

            }

        }).error(function (data) {
            // Error in Database.
            Console.log('Error in Database.');
        });
    }

});

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


});

homePortal.controller('DisciplinesController', function ($scope, $http, $timeout, $rootScope, $window, $location, ngProgress) {
    $scope.submitButton = false;
    $scope.loadingCad = false;
    $scope.submitButtonDel = false;
    $scope.resultShowCreate = false;
    $scope.resultShow = false;
    $scope.editorEnabled = [];
    $scope.Disciplina = [];
    $scope.resultShowEdit = false;

    $scope.getall = function () {
        
        //set progress bar
        ngProgress.color('#4285f4');
        ngProgress.start();

        $scope.loadingDisc = true;
        $http({
            method: 'POST',
            url: '/disciplines/getall',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).success(function (data) {
            if (data != null) {
                $rootScope.disciplinas = data;
                $scope.loadingDisc = false;
            } else {
                Console.log('erro.');
            }
        });

        //end progress
        ngProgress.complete();
    }

    $scope.enableEditor = function (id) {
        $scope.editorEnabled[id] = true;
    };

    $scope.disableEditor = function (id) {
        $scope.editorEnabled[id] = false;
    };

    $scope.save = function (id,idDiscipline) {
        $scope.editorEnabled[id] = false;

        var data = {
            idDiscipline: idDiscipline,
            name : $scope.Disciplina[id]
        }

        if (data.name != null && data.idDiscipline) {
            $http({
                method: 'POST',
                url: '/disciplines/update',
                data: $.param(data),  //param method from jQuery
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
            }).success(function (data) {
                console.log(data);
                if (data.success) { //success comes from the return json object
                    $scope.getall();
                } else {
                    $scope.resultMessageEdit = data.message;
                    $scope.resultEdit = 'bg-danger';
                    $scope.resultShowEdit = true;
                    $timeout(function () {
                        $scope.resultShowEdit = false;
                    }, 1500);
                }
            });
        } else {
            $scope.resultMessageEdit = "Estamos com problemas. Tente novamente.";
            $scope.resultEdit = 'bg-danger';
            $scope.resultShowEdit = true;
            $timeout(function () {
                $scope.resultShowEdit = false;
            }, 1500);
        }

    };

    $scope.submitCreateDiscipline = function (formCreateDiscipline) {
        $scope.submitButtonCad = true;
        if (formCreateDiscipline.$valid) {
            $scope.loadingCad = true;
            $http({
                method: 'POST',
                url: '/disciplines/create',
                data: $.param($scope.Discipline),  //param method from jQuery
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
            }).success(function (data) {
                console.log(data);
                if (data.success) { //success comes from the return json object
                    $scope.submitButtonCad = true;
                    $scope.resultMessageCad = data.message;
                    $scope.resultCad = 'bg-success';
                    $scope.loadingCad = false;
                    $scope.getall();
                    $scope.resultShowCreate = true;
                    $timeout(function () {
                        $scope.resultShowCreate = false;
                    }, 1500);

                } else {
                    $scope.submitButtonCad = false;
                    $scope.resultMessageCad = data.message;
                    $scope.resultCad = 'bg-danger';
                    $scope.loadingCad = false;
                    $scope.resultShowCreate = true;
                    $timeout(function () {
                        $scope.resultShowCreate = false;
                    }, 1500);
                }
            });
        } else {
            $scope.submitButtonCad = false;
            $scope.resultMessageCad = 'Campos obrigatórios. Preencha-os corretamente.';
            $scope.resultCad = 'bg-danger';
            $scope.loadingCad = false;
            $scope.resultShowCreate = true;
            $timeout(function () {
                $scope.resultShowCreate = false;
            }, 1500);
        }
    }

    $scope.submitDelete = function (id) {
        $scope.submitButtonDel = true;
        if (id != null) {
            $scope.loadingDel = true;
            $http({
                method: 'GET',
                url: '/disciplines/delete/'+id,
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
            }).success(function (data) {
                console.log(data);
                if (data.success) { //success comes from the return json object
                    $scope.submitButtonDel = true;
                    $scope.resultMessageDel = data.message;
                    $scope.resultDel = 'bg-success';
                    $scope.loadingDel = false;
                    $scope.getall();
                    $scope.resultShow = true;
                    $timeout(function () {
                        $scope.resultShow = false;
                    }, 1500);
                } else {
                    $scope.submitButtonDel = false;
                    $scope.resultMessageDel = data.message;
                    $scope.resultDel = 'bg-danger';
                    $scope.loadingDel = false;
                    $scope.resultShow = true;
                    $timeout(function () {
                        $scope.resultShow = false;
                    }, 1500);
                }
            });
        } else {
            $scope.submitButtonDel = false;
            $scope.resultMessageDel = 'Estamos com problemas, tente novamente.';
            $scope.resultDel = 'bg-danger';
            $scope.loadingDel = false;
            $scope.resultShow = true;
            $timeout(function () {
                $scope.resultShow = false;
            }, 1500);
        }
    }

    $scope.seeClasse = function (idDisc) {
        //set progress bar
        ngProgress.color('#4285f4');
        ngProgress.start();

        var url = '/classes/index?id=' + idDisc;
        $window.location.href = url;
    }
});

homePortal.controller('ClassesController', function ($scope, $http, $timeout, $rootScope, $location, $window, ngProgress) {
    $scope.resultShowCreateClass = false;
    $scope.loadingCadClass = false;
    $scope.editorEnabled = [];
    $scope.Class = [];
    $scope.submitButtonMessage = false;


    $scope.submitCreateClass = function (formCreateClass) {
        $scope.Classe.idDiscipline = location.search.split('id=')[1]
        
        $scope.submitButtonCadClass = true;
        if (formCreateClass.$valid) {
            $scope.loadingCadClass = true;
            $http({
                method: 'POST',
                url: '/classes/create',
                data: $.param($scope.Classe),  //param method from jQuery
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
            }).success(function (data) {
                console.log(data);
                if (data.success) { //success comes from the return json object
                    $scope.submitButtonCadClass = true;
                    $scope.resultMessageCadClass = data.message;
                    $scope.resultCadClass = 'bg-success';
                    $scope.loadingCadClass = false;
                    $rootScope.getallclasses($scope.Classe.idDiscipline);
                    $scope.resultShowCreateClass = true;
                    $timeout(function () {
                        $scope.resultShowCreateClass = false;
                    }, 1500);

                } else {
                    $scope.submitButtonCadClass = false;
                    $scope.resultMessageCadClass = data.message;
                    $scope.resultCadClass = 'bg-danger';
                    $scope.loadingCadClass = false;
                    $scope.resultShowCreateClass = true;
                    $timeout(function () {
                        $scope.resultShowCreateClass = false;
                    }, 1500);
                }
            });
        } else {
            $scope.submitButtonCadClass = false;
            $scope.resultMessageCadClass = 'Campos obrigatórios. Preencha-os corretamente.';
            $scope.resultCadClass = 'bg-danger';
            $scope.loadingCadClass = false;
            $scope.resultShowCreateClass = true;
            $timeout(function () {
                $scope.resultShowCreateClass = false;
            }, 1500);
        }
    }

    $scope.enableEditor = function (id) {
        $scope.editorEnabled[id] = true;
    };

    $scope.disableEditor = function (id) {
        $scope.editorEnabled[id] = false;
    };

    $scope.save = function (id, idClass, idDisc) {

        $scope.editorEnabled[id] = false;

        var data = {
            idClass: idClass,
            name: $scope.Class[id]
        }

        if (data.name != null && data.idClass) {
            $http({
                method: 'POST',
                url: '/classes/update',
                data: $.param(data),  //param method from jQuery
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
            }).success(function (data) {
                console.log(data);
                if (data.success) { //success comes from the return json object
                    $rootScope.getallclasses(idDisc);
                } else {
                    $scope.resultMessageEdit = data.message;
                    $scope.resultEdit = 'bg-danger';
                    $scope.resultShowEdit = true;
                    $timeout(function () {
                        $scope.resultShowEdit = false;
                    }, 1500);
                }
            });
        } else {
            $scope.resultMessageEdit = "Estamos com problemas. Tente novamente.";
            $scope.resultEdit = 'bg-danger';
            $scope.resultShowEdit = true;
            $timeout(function () {
                $scope.resultShowEdit = false;
            }, 1500);
        }

    };

    $scope.submitDelete = function (id) {

        var idDisc = location.search.split('id=')[1]

        if (id != null) {
            $scope.loadingDel = true;
            $http({
                method: 'GET',
                url: '/classes/delete/' + id,
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
            }).success(function (data) {
                console.log(data);
                if (data.success) { //success comes from the return json object
                    $scope.submitButtonDel = true;
                    $scope.resultMessageDel = data.message;
                    $scope.resultDel = 'bg-success';
                    $scope.loadingDel = false;
                    $rootScope.getallclasses(idDisc);
                    $scope.resultShow = true;
                    $timeout(function () {
                        $scope.resultShow = false;
                    }, 1500);
                } else {
                    $scope.submitButtonDel = false;
                    $scope.resultMessageDel = data.message;
                    $scope.resultDel = 'bg-danger';
                    $scope.loadingDel = false;
                    $scope.resultShow = true;
                    $timeout(function () {
                        $scope.resultShow = false;
                    }, 1500);
                }
            });
        } else {
            $scope.submitButtonDel = false;
            $scope.resultMessageDel = 'Estamos com problemas, tente novamente.';
            $scope.resultDel = 'bg-danger';
            $scope.loadingDel = false;
            $scope.resultShow = true;
            $timeout(function () {
                $scope.resultShow = false;
            }, 1500);
        }
    }

    $scope.getall = function () {
        var id = location.search.split('?id=')[1];
        $scope.loadingClass = true;
        $rootScope.getallclasses(id);
        $scope.loadingClass = false;

        //end progress bar
        ngProgress.complete();
    }

    $scope.getOneClass = function (pIdClass) {
        var url = '/classes/manageclass?id=' + pIdClass;
        $window.location.href = url;
    }

    $scope.submitPostClasse = function (formPost) {
        
        //set progress bar
        ngProgress.color('#4285f4');
        ngProgress.start();

        //get id classe
        var idClasse = location.search.split('?id=')[1];
        //get message
        var message = $scope.Post.message;
        //get file
        var file = document.getElementById('file').files[0];

        //url send form
        var uploadUrl = "/classes/uploadPost";

        if (file != null && formPost.$valid) {
            var fd = new FormData();
            fd.append('file', file);
            fd.append('message', message);
            fd.append('idClasse', idClasse);
            $http.post(uploadUrl, fd, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
            .success(function () {
                $timeout(function () {
                    ngProgress.complete();
                    $rootScope.getDataOneClass();
                    $scope.Post.message = "";
                    $scope.Post.file = "";
                    $scope.resultMessagePost = 'Postagem enviada com sucesso.';
                    $scope.resultMessageClassPost = 'bg-success';
                    $scope.resultShowMessagePost = true;
                    $timeout(function () {
                        $scope.resultShowMessagePost = false;
                    }, 2000);
                }, 2000);
            })
            .error(function () {
            });
        }
        else {
            ngProgress.complete();
            $scope.resultMessagePost = 'Nenhum anexo encontrado. Se você deseja enviar uma mensagem apenas, utilize a opção Recado para turma.';
            $scope.resultMessageClassPost = 'bg-danger';
            $scope.resultShowMessagePost = true;
            $timeout(function () {
                $scope.resultShowMessagePost = false;
            }, 10000);
        }
        
    }

    $scope.download = function (pUrl)
    {
        var dataParam = { url: pUrl };

        $http({
            method: 'POST',
            url: '/classes/download',
            data: $.param(dataParam),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
        });
    }

    $scope.submitMessage = function (form) {

        var idClasse = location.search.split('id=')[1];

        var dataParam = {
            id: idClasse,
            message : $scope.Classe.message
        }

        $scope.submitButtonCadClass = true;
        if (form.$valid) {
            $scope.loadingMessage = true;
            $http({
                method: 'POST',
                url: '/classes/invitemessage',
                data: $.param(dataParam),  //param method from jQuery
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
            }).success(function (data) {
                if (data.success) { //success comes from the return json object
                    $scope.submitButtonMessage = true;
                    $scope.resultMessage = data.message;
                    $scope.resultMessageClass = 'bg-success';
                    $scope.loadingMessage = false;
                    $scope.resultShowMessage = true;
                    $timeout(function () {
                        $scope.resultShowMessage = false;
                    }, 1500);
                } else {
                    $scope.submitButtonMessage = false;
                    $scope.resultMessage = data.message;
                    $scope.resultMessageClass = 'bg-danger';
                    $scope.loadingMessage = false;
                    $scope.resultShowMessage = true;
                    $timeout(function () {
                        $scope.resultShowMessage = false;
                    }, 1500);
                }
            });
        } else {
            $scope.submitButtonMessage = false;
            $scope.resultMessage = 'Campos obrigatórios. Preencha-os corretamente.';
            $scope.resultMessageClass = 'bg-danger';
            $scope.loadingMessage = false;
            $scope.resultShowMessage = true;
            $timeout(function () {
                $scope.resultShowMessage = false;
            }, 3000);
        }

    }

    $scope.deletePost = function (id) {

        $http({
            method: 'GET',
            url: '/classes/deletePost/' + id,
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' } 
        }).success(function (data) {

            if (data.success)
            {
                var IdClass = location.search.split('?id=')[1];
                $rootScope.getDataOneClass();
                $location.href = "classes/manageclass?id=" + IdClass;
            }

        }).error(function (data) {

        });
    }

});

homePortal.controller('AutoCompleteController', function ($scope, $http, $rootScope, $timeout) {

    $scope.completing = false;
    $scope.dicas = [];

    $scope.pesquisar = function (search) {
        // Se a pesquisa for vazia
        if (search == "") {
            // Retira o autocomplete
            $scope.completing = false;

        } else {

            var dataParam = {
                name: search
            }

            if (search != null && search.length >= 3) {
                $scope.loadingSearch = true;
                $http({
                    method: 'POST',
                    url: '/home/searchteacher',
                    data: $.param(dataParam),
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
                }).success(function (data) {
                    console.log(data);
                    $scope.loadingSearch = false;
                    // Habilita o campo
                    $scope.completing = true;
                    // JSON retornado do banco

                    $scope.dicas = data;

                }).error(function (data) {
                    // Error in Database.
                    Console.log('Error in Database.');
                });
            } 
        }
    };

    $scope.pesquisarAlunos = function (search) {
        // Se a pesquisa for vazia
        if (search == "") {
            // Retira o autocomplete
            $scope.completing = false;

        } else {

            var dataParam = {
                name: search
            }

            if (search != null && search.length >= 3) {
                $scope.loadingSearch = true;
                $http({
                    method: 'POST',
                    url: '/home/searchstudent',
                    data: $.param(dataParam),
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
                }).success(function (data) {
                    $scope.loadingSearch = false;
                    // Habilita o campo
                    $scope.completing = true;
                    // JSON retornado do banco

                    $scope.dicas = data;

                }).error(function (data) {
                    // Error in Database.
                    Console.log('Error in Database.');
                });
            }
        }
    };

    $scope.addStudent = function (idStd) {
        var pIdClass = location.search.split('?id=')[1];

        var dataParam = {
            idStudent: idStd,
            idClass: pIdClass
        }

        if (idStd != null) {
            $http({
                method: 'POST',
                url: '/classes/insertstudentinclasse/',
                data: $.param(dataParam),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  
            }).success(function (data) {
                if (data.success) {
                    $scope.completing = false;
                    $scope.search = "";
                    $rootScope.getDataOneClass();
                    $scope.resultMessageAddStudent = data.message;
                    $scope.resultClassAddStudent = 'bg-success';
                    $scope.resultShowAddStudent = true;
                    $timeout(function () {
                        $scope.resultShowAddStudent = false;
                    }, 1500);
                }
                else {
                    $scope.resultMessageAddStudent = data.message;
                    $scope.resultClassAddStudent = 'bg-danger';
                    $scope.resultShowAddStudent = true;
                    $timeout(function () {
                        $scope.resultShowAddStudent = false;
                    }, 1500);
                }

            }).error(function (data) {
                console.log('Error in Database.')
            });
        }

    }

});

homePortal.controller('IndexController', function ($scope, $http, $rootScope, $interval, $timeout, ngProgress) {
    
    $interval(function () {
        $scope.getNumMessages();
    }, 5000);

    var count = 0;

    $scope.getNewPosts = function (idStudent) {

        dataParam = {
            id: idStudent
        }

        $http({
            method: 'POST',
            url: '/classes/getpoststeachers',
            data: $.param(dataParam),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
        }).success(function (data) {
            //console.log(data);
            $scope.publicationsTeachers = data;
            $scope.getNumMessages(idStudent);
        }).error(function (data) {
            // Error in Database.
            Console.log('Error in Database.');
        });
    }

    $scope.getNumMessages = function () {

        $http({
            method: 'POST',
            url: '/message/getNumMessageStudent',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  
        }).success(function (data) {
            //console.log(data);

            $scope.numMessages = data;

            if (data != 0 && data != count) {
                $scope.badgeMessage = "badge";
                 count = data;

                if (!Notification) {
                    alert('Notifications are supported in modern versions of Chrome, Firefox and Opera.');
                    return;
                }

                if (Notification.permission !== "granted")
                    Notification.requestPermission();

                var notification = new Notification('Portal do conhecimento', {
                    icon: '/Content/images/icon_notif.png',
                    body: "Existem novas mensagens para você!",
                });

                //notification.onclick = function () {
                //    window.open("http://stackoverflow.com/a/13328397/1269037");
                //};
            }
            else {
                $scope.badgeMessage = "badge";
            }

        }).error(function (data) {
            // Error in Database.
            Console.log('Error in Database.');
        });
    }

    $scope.getNewMessages = function (idStudent) {
        $rootScope.getMessages(idStudent);
    }

    $scope.initPage = function (id) {
        //set progress bar
        ngProgress.color('#4285f4');
        ngProgress.start();

        $scope.getNewPosts(id);

        //end progress
        ngProgress.complete();
    }

    $scope.initPageTeacher = function () {
        $rootScope.getPublicationsTeacherProf();
    }

    $scope.submitPostTeacher = function (formPost) {

        //set progress bar
        ngProgress.color('#4285f4');
        ngProgress.start();

        //get message
        var message = $scope.Post.message;
        //get file
        var file = document.getElementById('file').files[0];

        //url send form
        var uploadUrl = "/teachers/uploadPost";

        if (formPost.$valid) {
            //Post com anexos
            if (file != null) {

                var fd = new FormData();
                fd.append('file', file);
                fd.append('message', message);

                $http.post(uploadUrl, fd, {
                    transformRequest: angular.identity,
                    headers: { 'Content-Type': undefined }
                })
                .success(function () {
                    //$rootScope.getDataOneClass();
                    //set progress bar
                    $timeout(function () {
                        ngProgress.complete();
                        $scope.Post.message = "";
                        $scope.Post.file = "";
                        $scope.resultMessagePost = 'Postagem enviada com sucesso.';
                        $scope.resultMessageClassPost = 'bg-success';
                        $scope.resultShowMessagePost = true;
                        $timeout(function () {
                            $scope.resultShowMessagePost = false;
                        }, 2000);
                    }, 2000);
                })
                .error(function () {
                });
            //Post sem anexo
            } else {

                var fd = new FormData();
                fd.append('message', message);

                $http.post(uploadUrl, fd, {
                    transformRequest: angular.identity,
                    headers: { 'Content-Type': undefined }
                })
                .success(function () {
                    //$rootScope.getDataOneClass();
                    //set progress bar
                    $timeout(function () {
                        ngProgress.complete();
                        $scope.Post.message = "";
                        $scope.Post.file = "";
                        $scope.resultMessagePost = 'Postagem enviada com sucesso sem anexos.';
                        $scope.resultMessageClassPost = 'bg-success';
                        $scope.resultShowMessagePost = true;
                        $timeout(function () {
                            $scope.resultShowMessagePost = false;
                        }, 2000);
                    }, 2000);
                })
                .error(function () {
                });

            }
        }
        else {
            $scope.resultMessagePost = 'Ops, estamos com problemas. Tente novamente.';
            $scope.resultMessageClassPost = 'bg-danger';
            $scope.resultShowMessagePost = true;
            $timeout(function () {
                $scope.resultShowMessagePost = false;
            }, 10000);
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

followerModule.controller('FollowerController', function ($scope, $http, $timeout, ngProgress) {

    $scope.checkFollow = function (idFollower, idFollowing) {

        var dataParam = {
            idFollower: idFollower,
            idFollowing: idFollowing
        }

        if (idFollower != idFollowing) {
            $http({
                method: 'POST',
                url: '/home/checkfollow',
                data: $.param(dataParam),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (data) {
                console.log(data);
                if (data.success) {
                    $scope.class = 'bg-success';
                    $scope.textFollow = "Seguindo";
                    $scope.seguindoIcon = true;
                    $scope.seguirIcon = false;
                    $scope.itsMe = false;

                } else {
                    $scope.class = 'bg-danger';
                    $scope.textFollow = "Seguir";
                    $scope.seguirIcon = true;
                    $scope.seguindoIcon = false;
                    $scope.itsMe = false;
                }
            });

        }
        else {
            $scope.itsMe = true;
        }

    }

    $scope.follow = function (idFollower, idFollowing) {

        var dataParam = {
            idFollower: idFollower,
            idFollowing: idFollowing
        }

        if (idFollower != idFollowing) {
            $http({
                method: 'POST',
                url: '/home/checkfollow',
                data: $.param(dataParam),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (data) {
                console.log(data);
                if (data.success) {
                    $scope.itsMe = false;
                    $http({
                        method: 'POST',
                        url: '/home/unfollow',
                        data: $.param(dataParam),
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
                    }).success(function (data) {
                        console.log(data);
                        if (data.success) {
                            $scope.class = 'bg-success';
                            $scope.textFollow = "Seguir";
                            $scope.seguirIcon = true;
                            $scope.seguindoIcon = false;

                        }
                    });

                }
                else
                {
                    $scope.itsMe = false;
                    $http({
                        method: 'POST',
                        url: '/home/follow',
                        data: $.param(dataParam),
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
                    }).success(function (data) {
                        console.log(data);
                        if (data.success) {
                            $scope.class = 'bg-success';
                            $scope.textFollow = "Seguindo";
                            $scope.seguindoIcon = true;
                            $scope.seguirIcon = false;
                        }
                    });

                }
            });

        }
        else {
            $scope.itsMe = true;
        }

    }

    $scope.getFollowers = function (idFollower) {
        //set progress bar
        ngProgress.color('#4285f4');
        ngProgress.start();

        var dataParam = {
            id: idFollower
        }

        if (idFollower != null) {
            $http({
                method: 'POST',
                url: '/home/getfollowers',
                data: $.param(dataParam),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
            }).success(function (data) {
                console.log(data);
                
                $scope.followers = data;
                
                //end progress bar
                ngProgress.complete();

            }).error(function (data) {

                //end progress bar
                ngProgress.complete();
                // Error in Database.
                Console.log('Error in Database.');
            });
        }
    }

    $scope.getFollowersOther = function (idFollower) {

        //set progress bar
        ngProgress.color('#4285f4');
        ngProgress.start();

        var dataParam = {
            id: idFollower
        }

        if (idFollower != null) {
            $http({
                method: 'POST',
                url: '/home/getfollowersother',
                data: $.param(dataParam),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }  //set the headers so angular passing info as form data (not request payload)
            }).success(function (data) {
                console.log(data);

                $scope.followersOther = data;

                //end progress bar
                ngProgress.complete();

            }).error(function (data) {

                //end progress bar
                ngProgress.complete();
                // Error in Database.
                Console.log('Error in Database.');
            });
        }
    }

});