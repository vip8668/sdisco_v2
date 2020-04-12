(function () {
    $(function () {

        var _$transPortdetailsTable = $('#TransPortdetailsTable');
        var _transPortdetailsService = abp.services.app.transPortdetails;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.TransPortdetails.Create'),
            edit: abp.auth.hasPermission('Pages.TransPortdetails.Edit'),
            'delete': abp.auth.hasPermission('Pages.TransPortdetails.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/TransPortdetails/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/TransPortdetails/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditTransPortdetailModal'
        });

		 var _viewTransPortdetailModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/TransPortdetails/ViewtransPortdetailModal',
            modalClass: 'ViewTransPortdetailModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$transPortdetailsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _transPortdetailsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#TransPortdetailsTableFilter').val(),
					fromFilter: $('#FromFilterId').val(),
					toFilter: $('#ToFilterId').val(),
					minTotalSeatFilter: $('#MinTotalSeatFilterId').val(),
					maxTotalSeatFilter: $('#MaxTotalSeatFilterId').val(),
					isTaxiFilter: $('#IsTaxiFilterId').val(),
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
                                    _viewTransPortdetailModal.open({ id: data.record.transPortdetail.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.transPortdetail.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteTransPortdetail(data.record.transPortdetail);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "transPortdetail.from",
						 name: "from"   
					},
					{
						targets: 2,
						 data: "transPortdetail.to",
						 name: "to"   
					},
					{
						targets: 3,
						 data: "transPortdetail.totalSeat",
						 name: "totalSeat"   
					},
					{
						targets: 4,
						 data: "transPortdetail.isTaxi",
						 name: "isTaxi"  ,
						render: function (isTaxi) {
							if (isTaxi) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 5,
						 data: "productName" ,
						 name: "productFk.name" 
					}
            ]
        });

        function getTransPortdetails() {
            dataTable.ajax.reload();
        }

        function deleteTransPortdetail(transPortdetail) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _transPortdetailsService.delete({
                            id: transPortdetail.id
                        }).done(function () {
                            getTransPortdetails(true);
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

        $('#CreateNewTransPortdetailButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _transPortdetailsService
                .getTransPortdetailsToExcel({
				filter : $('#TransPortdetailsTableFilter').val(),
					fromFilter: $('#FromFilterId').val(),
					toFilter: $('#ToFilterId').val(),
					minTotalSeatFilter: $('#MinTotalSeatFilterId').val(),
					maxTotalSeatFilter: $('#MaxTotalSeatFilterId').val(),
					isTaxiFilter: $('#IsTaxiFilterId').val(),
					productNameFilter: $('#ProductNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditTransPortdetailModalSaved', function () {
            getTransPortdetails();
        });

		$('#GetTransPortdetailsButton').click(function (e) {
            e.preventDefault();
            getTransPortdetails();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getTransPortdetails();
		  }
		});
    });
})();