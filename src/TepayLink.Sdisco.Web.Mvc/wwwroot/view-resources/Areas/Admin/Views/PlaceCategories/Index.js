(function () {
    $(function () {

        var _$placeCategoriesTable = $('#PlaceCategoriesTable');
        var _placeCategoriesService = abp.services.app.placeCategories;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.PlaceCategories.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.PlaceCategories.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.PlaceCategories.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/PlaceCategories/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/PlaceCategories/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPlaceCategoryModal'
        });

		 var _viewPlaceCategoryModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/PlaceCategories/ViewplaceCategoryModal',
            modalClass: 'ViewPlaceCategoryModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$placeCategoriesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _placeCategoriesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#PlaceCategoriesTableFilter').val(),
					nameFilter: $('#NameFilterId').val()
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
                                    _viewPlaceCategoryModal.open({ id: data.record.placeCategory.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.placeCategory.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deletePlaceCategory(data.record.placeCategory);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "placeCategory.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "placeCategory.image",
						 name: "image"   
					},
					{
						targets: 3,
						 data: "placeCategory.icon",
						 name: "icon"   
					}
            ]
        });

        function getPlaceCategories() {
            dataTable.ajax.reload();
        }

        function deletePlaceCategory(placeCategory) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _placeCategoriesService.delete({
                            id: placeCategory.id
                        }).done(function () {
                            getPlaceCategories(true);
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

        $('#CreateNewPlaceCategoryButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _placeCategoriesService
                .getPlaceCategoriesToExcel({
				filter : $('#PlaceCategoriesTableFilter').val(),
					nameFilter: $('#NameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPlaceCategoryModalSaved', function () {
            getPlaceCategories();
        });

		$('#GetPlaceCategoriesButton').click(function (e) {
            e.preventDefault();
            getPlaceCategories();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getPlaceCategories();
		  }
		});
    });
})();