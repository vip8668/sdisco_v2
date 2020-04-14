(function () {
    $(function () {

        var _$productSchedulesTable = $('#ProductSchedulesTable');
        var _productSchedulesService = abp.services.app.productSchedules;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.ProductSchedules.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.ProductSchedules.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.ProductSchedules.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductSchedules/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ProductSchedules/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditProductScheduleModal'
        });

		 var _viewProductScheduleModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductSchedules/ViewproductScheduleModal',
            modalClass: 'ViewProductScheduleModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$productSchedulesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _productSchedulesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ProductSchedulesTableFilter').val(),
					productNameFilter: $('#ProductNameFilterId').val()
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
                                    _viewProductScheduleModal.open({ id: data.record.productSchedule.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.productSchedule.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteProductSchedule(data.record.productSchedule);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "productSchedule.totalSlot",
						 name: "totalSlot"   
					},
					{
						targets: 2,
						 data: "productSchedule.totalBook",
						 name: "totalBook"   
					},
					{
						targets: 3,
						 data: "productSchedule.lockedSlot",
						 name: "lockedSlot"   
					},
					{
						targets: 4,
						 data: "productSchedule.tripLength",
						 name: "tripLength"   
					},
					{
						targets: 5,
						 data: "productSchedule.note",
						 name: "note"   
					},
					{
						targets: 6,
						 data: "productSchedule.price",
						 name: "price"   
					},
					{
						targets: 7,
						 data: "productSchedule.ticketPrice",
						 name: "ticketPrice"   
					},
					{
						targets: 8,
						 data: "productSchedule.costPrice",
						 name: "costPrice"   
					},
					{
						targets: 9,
						 data: "productSchedule.hotelPrice",
						 name: "hotelPrice"   
					},
					{
						targets: 10,
						 data: "productSchedule.startDate",
						 name: "startDate" ,
					render: function (startDate) {
						if (startDate) {
							return moment(startDate).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 11,
						 data: "productSchedule.endDate",
						 name: "endDate" ,
					render: function (endDate) {
						if (endDate) {
							return moment(endDate).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 12,
						 data: "productSchedule.departureTime",
						 name: "departureTime"   
					},
					{
						targets: 13,
						 data: "productSchedule.revenue",
						 name: "revenue"   
					},
					{
						targets: 14,
						 data: "productSchedule.allowBook",
						 name: "allowBook"  ,
						render: function (allowBook) {
							if (allowBook) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 15,
						 data: "productName" ,
						 name: "productFk.name" 
					}
            ]
        });

        function getProductSchedules() {
            dataTable.ajax.reload();
        }

        function deleteProductSchedule(productSchedule) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _productSchedulesService.delete({
                            id: productSchedule.id
                        }).done(function () {
                            getProductSchedules(true);
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

        $('#CreateNewProductScheduleButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _productSchedulesService
                .getProductSchedulesToExcel({
				filter : $('#ProductSchedulesTableFilter').val(),
					productNameFilter: $('#ProductNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditProductScheduleModalSaved', function () {
            getProductSchedules();
        });

		$('#GetProductSchedulesButton').click(function (e) {
            e.preventDefault();
            getProductSchedules();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getProductSchedules();
		  }
		});
    });
})();