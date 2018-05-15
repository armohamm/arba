(function (app) {
    var LogsController = function ($scope, $http) {
        $http.get("/api/Rest/GetLogs").then(function (result) {
            $scope.logs = result.data;
        });

        $scope.message = "bla bla bla";
    };

    app.controller("LogsController", LogsController);

}(angular.module("logs")));


