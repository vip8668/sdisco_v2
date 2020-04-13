(function () {
    $(function () {

        var _$categoriesTable = $('#CategoriesTable');
        var _categoriesService = abp.services.app.categories;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.Categories.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.Categories.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.Categories.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Categories/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Categories/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCategoryModal'
        });

		 var _viewCategoryModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Categories/ViewcategoryModal',
            modalClass: 'ViewCategoryModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$categoriesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _categoriesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#CategoriesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					productTypeFilter: $('#ProductTypeFilterId').val(),
					minOrderFilter: $('#MinOrderFilterId').val(),
					maxOrderFilter: $('#MaxOrderFilterId').val()
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
                                    _viewCategoryModal.open({ id: data.record.category.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.category.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteCategory(data.record.category);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "category.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "category.image",
						 name: "image"   
					},
					{
						targets: 3,
						 data: "category.icon",
						 name: "icon"   
					},
					{
						targets: 4,
						 data: "category.productType",
						 name: "productType"   ,
						render: function (productType) {
							return app.localize('Enum_ProductTypeEnum_' + productType);
						}
			
					},
					{
						targets: 5,
						 data: "category.order",
						 name: "order"   
					}
            ]
        });

        function getCategories() {
            dataTable.ajax.reload();
        }

        function deleteCategory(category) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _categoriesService.delete({
                            id: category.id
                        }).done(function () {
                            getCategories(true);
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

        $('#CreateNewCategoryButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _categoriesService
                .getCategoriesToExcel({
				filter : $('#CategoriesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					productTypeFilter: $('#ProductTypeFilterId').val(),
					minOrderFilter: $('#MinOrderFilterId').val(),
					maxOrderFilter: $('#MaxOrderFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCategoryModalSaved', function () {
            getCategories();
        });

		$('#GetCategoriesButton').click(function (e) {
            e.preventDefault();
            getCategories();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getCategories();
		  }
		});
    });
})();