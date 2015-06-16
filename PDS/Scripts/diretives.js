//Diretiva para fazer o upload de um atributo file
homePortal.directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;

            element.bind('change', function () {
                scope.$apply(function () {
                    modelSetter(scope, element[0].files[0]);
                });
            });
        }
    };
}]);

//Diretiva para verificar o scroll da página para carregar os itens
homePortal.directive('whenScrolled', function () {
    return function (scope, elm, attr) {
        var raw = elm[0];

        elm.bind('scroll', function () {
            if (raw.scrollTop + raw.offsetHeight >= raw.scrollHeight) {
                scope.$apply(attr.whenScrolled);
            }
        });
    };
});

//Diretiva das Estrelinhas
starModule.directive('starRating', function () {
    return {
        restrict: 'A',
        template: '<ul class="rating">' +
            '<li ng-repeat="star in stars" ng-class="star" ng-click="toggle($index)">' +
            '<span style="font-size: 35px;" class="glyphicon glyphicon-star"></span>' +
            '</li>' +
            '</ul>',
        scope: {
            ratingValue: '=',
            max: '=',
            onRatingSelected: '&'
        },
        link: function (scope, elem, attrs) {

            var updateStars = function () {
                scope.stars = [];
                for (var i = 0; i < scope.max; i++) {
                    scope.stars.push({
                        filled: i < scope.ratingValue
                    });
                }
            };

            scope.toggle = function (index) {
                scope.ratingValue = index + 1;
                scope.onRatingSelected({
                    rating: index + 1
                });
            };

            scope.$watch('ratingValue', function (oldVal, newVal) {
                if (newVal) {
                    updateStars();
                }
            });
        }
    }
});

//Diretiva para mascara de data
homeSite.directive('uiDate', [function () {
    return {
        require: "ngModel",
        link: function (scope, element, attrs, ctrl) {
            var _formatDate = function (date) {

                date = date.replace(/[^0-9]+/g, "");

                if (date.length > 2) {
                    date = date.substring(0, 2) + "/" + date.substring(2);
                }

                if (date.length > 5) {
                    date = date.substring(0, 5) + "/" + date.substring(5, 9);
                }

                if (date.length < 10) {
                    ctrl.$setValidity("unique", false);
                }
                else {
                    ctrl.$setValidity("unique", true);
                }

                return date;
            };

            element.bind("keyup", function () {

                ctrl.$setViewValue(_formatDate(ctrl.$viewValue));
                ctrl.$render();

            });

        }
    }
}]);

//Diretiva para mascara de data
manageModule.directive('uiDate', [function () {
    return {
        require: "ngModel",
        link: function (scope, element, attrs, ctrl) {
            var _formatDate = function (date) {

                date = date.replace(/[^0-9]+/g, "");

                if (date.length > 2) {
                    date = date.substring(0, 2) + "/" + date.substring(2);
                }

                if (date.length > 5) {
                    date = date.substring(0, 5) + "/" + date.substring(5, 9);
                }

                if (date.length < 10) {
                    ctrl.$setValidity("unique", false);
                }
                else {
                    ctrl.$setValidity("unique", true);
                }

                return date;
            };

            element.bind("keyup", function () {

                ctrl.$setViewValue(_formatDate(ctrl.$viewValue));
                ctrl.$render();

            });

        }
    }
}]);

//Diretiva para mascara de data
accountModule.directive('uiDate', [function () {
    return {
        require: "ngModel",
        link: function (scope, element, attrs, ctrl) {
            var _formatDate = function (date) {

                date = date.replace(/[^0-9]+/g, "");

                if (date.length > 2) {
                    date = date.substring(0, 2) + "/" + date.substring(2);
                }

                if (date.length > 5) {
                    date = date.substring(0, 5) + "/" + date.substring(5, 9);
                }

                if (date.length < 10) {
                    ctrl.$setValidity("unique", false);
                }
                else {
                    ctrl.$setValidity("unique", true);
                }

                return date;
            };

            element.bind("keyup", function () {

                ctrl.$setViewValue(_formatDate(ctrl.$viewValue));
                ctrl.$render();

            });

        }
    }
}]);