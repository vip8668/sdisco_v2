(function () {
    $(function () {

        var _$placesTable = $('#PlacesTable');
        var _placesService = abp.services.app.places;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.Places.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.Places.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.Places.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Places/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Places/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPlaceModal'
        });

		 var _viewPlaceModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Places/ViewplaceModal',
            modalClass: 'ViewPlaceModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$placesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _placesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#PlacesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					detinationNameFilter: $('#DetinationNameFilterId').val(),
					placeCategoryNameFilter: $('#PlaceCategoryNameFilterId').val()
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
                                    _viewPlaceModal.open({ id: data.record.place.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.place.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deletePlace(data.record.place);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "place.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "place.displayAddress",
						 name: "displayAddress"   
					},
					{
						targets: 3,
						 data: "place.googleAddress",
						 name: "googleAddress"   
					},
					{
						targets: 4,
						 data: "place.overview",
						 name: "overview"   
					},
					{
						targets: 5,
						 data: "place.whatToExpect",
						 name: "whatToExpect"   
					},
					{
						targets: 6,
						 data: "detinationName" ,
						 name: "detinationFk.name" 
					},
					{
						targets: 7,
						 data: "placeCategoryName" ,
						 name: "placeCategoryFk.name" 
					}
            ]
        });

        function getPlaces() {
            dataTable.ajax.reload();
        }

        function deletePlace(place) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _placesService.delete({
                            id: place.id
                        }).done(function () {
                            getPlaces(true);
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

        $('#CreateNewPlaceButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _placesService
                .getPlacesToExcel({
				filter : $('#PlacesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					detinationNameFilter: $('#DetinationNameFilterId').val(),
					placeCategoryNameFilter: $('#PlaceCategoryNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPlaceModalSaved', function () {
            getPlaces();
        });

		$('#GetPlacesButton').click(function (e) {
            e.preventDefault();
            getPlaces();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getPlaces();
		  }
		});
    });
})();