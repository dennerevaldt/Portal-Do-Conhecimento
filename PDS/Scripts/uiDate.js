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
                    date = date.substring(0, 5) + "/" + date.substring(5,9);
                }

                if (date.length < 10 )
                {
                    ctrl.$setValidity("unique", false);
                }
                else
                {
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