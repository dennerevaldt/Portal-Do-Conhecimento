homePortal.service('fileUpload', ['$http', function ($http, $rootScope) {
    this.uploadFileToUrl = function (file, uploadUrl, message, idClasse) {
        var fd = new FormData();
        fd.append('file', file);
        fd.append('message', message);
        fd.append('idClasse', idClasse);
        $http.post(uploadUrl, fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })
        .success(function () {
            $rootScope.getDataOneClass();
        })
        .error(function () {
        });
    }
}]);