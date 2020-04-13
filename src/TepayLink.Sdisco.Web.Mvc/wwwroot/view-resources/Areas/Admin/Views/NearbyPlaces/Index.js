(function () {
    $(function () {

        var _$nearbyPlacesTable = $('#NearbyPlacesTable');
        var _nearbyPlacesService = abp.services.app.nearbyPlaces;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.NearbyPlaces.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.NearbyPlaces.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.NearbyPlaces.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/NearbyPlaces/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/NearbyPlaces/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditNearbyPlaceModal'
        });

		 var _viewNearbyPlaceModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/NearbyPlaces/ViewnearbyPlaceModal',
            modalClass: 'ViewNearbyPlaceModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$nearbyPlacesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _nearbyPlacesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#NearbyPlacesTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					placeNameFilter: $('#PlaceNameFilterId').val(),
					placeName2Filter: $('#PlaceName2FilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    width: 120,
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
						{
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewNearbyPlaceModal.open({ id: data.record.nearbyPlace.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.nearbyPlace.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteNearbyPlace(data.record.nearbyPlace);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "nearbyPlace.description",
						 name: "description"   
					},
					{
						targets: 2,
						 data: "placeName" ,
						 name: "placeFk.name" 
					},
					{
						targets: 3,
						 data: "placeName2" ,
						 name: "nearbyPlaceFk.name" 
					}
            ]
        });

        function getNearbyPlaces() {
            dataTable.ajax.reload();
        }

        function deleteNearbyPlace(nearbyPlace) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _nearbyPlacesService.delete({
                            id: nearbyPlace.id
                        }).done(function () {
                            getNearbyPlaces(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

		$('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewNearbyPlaceButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _nearbyPlacesService
                .getNearbyPlacesToExcel({
				filter : $('#NearbyPlacesTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					placeNameFilter: $('#PlaceNameFilterId').val(),
					placeName2Filter: $('#PlaceName2FilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditNearbyPlaceModalSaved', function () {
            getNearbyPlaces();
        });

		$('#GetNearbyPlacesButton').click(function (e) {
            e.preventDefault();
            getNearbyPlaces();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getNearbyPlaces();
		  }
		});
    });
})();