var domicileApp = angular.module("domicileApp", ["ngRoute", "ngMaterial", "mdDataTable"]);

domicileApp.config(function($routeProvider) {
    $routeProvider
        .when("/services",
        {
            templateUrl: "/Content/Views/Partials/services.html",
            controller: "ServicesController"
        });
});

domicileApp.controller("ServicesController",
    function ($scope, $http, $mdDialog) {

        // Get All Services
        $http({ method: "GET", url: "/api/services" })
            .then(function(response) {
                $scope.services = response.data;
            }, function(error) {
                console.log("Failed getting services", error);
            });

        $scope.updateService = function (service) {
            // TODO: Create method to update a service
            // Updating a service simply means that any of the service variables have been altered.



            $http({ method: "PUT", url: "/api/services", data: service })
                .then(function(data) {
                    $scope.services.push(data);
                });
        };

        $scope.viewService = function (service) {
            console.log(service);

            $http({ method: "GET", url: "/api/services/" + service.name })
                .then(function(response) {
                        $mdDialog.show({
                            controller: "DialogController",
                            templateUrl: "/Content/Views/Partials/Dialogs/detailed-service.html",
                            parent: angular.element(document.body),
                            clickOutsideToClose: true,
                            locals: { service: service }
                        });
                    },
                    function(error) {
                        console.log("Error trying to view service", error);
                    });
        };

    });

domicileApp.controller("DialogController",
    function ($scope, $mdDialog, service) {
        $scope.service = service;
       
        $scope.$watch('filterText', function () {
            console.log("TEXT ALTERED");
        });

        $scope.hide = function() {
            $mdDialog.hide();
        };

        $scope.cancel = function() {
            $mdDialog.cancel();
        };
    });