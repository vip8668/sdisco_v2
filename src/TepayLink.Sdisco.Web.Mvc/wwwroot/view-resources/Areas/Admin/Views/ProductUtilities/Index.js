(function () {
    $(function () {

        var _$productUtilitiesTable = $('#ProductUtilitiesTable');
        var _productUtilitiesService = abp.services.app.productUtilities;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.ProductUtilities.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.ProductUtilities.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.ProductUtilities.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductUtilities/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ProductUtilities/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditProductUtilityModal'
        });

		 var _viewProductUtilityModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductUtilities/ViewproductUtilityModal',
            modalClass: 'ViewProductUtilityModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$productUtilitiesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _productUtilitiesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ProductUtilitiesTableFilter').val(),
					productNameFilter: $('#ProductNameFilterId').val(),
					utilityNameFilter: $('#UtilityNameFilterId').val()
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
                                    _viewProductUtilityModal.open({ id: data.record.productUtility.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.productUtility.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteProductUtility(data.record.productUtility);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "productName" ,
						 name: "productFk.name" 
					},
					{
						targets: 2,
						 data: "utilityName" ,
						 name: "utilityFk.name" 
					}
            ]
        });

        function getProductUtilities() {
            dataTable.ajax.reload();
        }

        function deleteProductUtility(productUtility) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _productUtilitiesService.delete({
                            id: productUtility.id
                        }).done(function () {
                            getProductUtilities(true);
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

        $('#CreateNewProductUtilityButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _productUtilitiesService
                .getProductUtilitiesToExcel({
				filter : $('#ProductUtilitiesTableFilter').val(),
					productNameFilter: $('#ProductNameFilterId').val(),
					utilityNameFilter: $('#UtilityNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditProductUtilityModalSaved', function () {
            getProductUtilities();
        });

		$('#GetProductUtilitiesButton').click(function (e) {
            e.preventDefault();
            getProductUtilities();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getProductUtilities();
		  }
		});
    });
})();