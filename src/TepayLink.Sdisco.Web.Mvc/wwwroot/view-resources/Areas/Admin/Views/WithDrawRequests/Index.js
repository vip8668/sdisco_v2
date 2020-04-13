(function () {
    $(function () {

        var _$withDrawRequestsTable = $('#WithDrawRequestsTable');
        var _withDrawRequestsService = abp.services.app.withDrawRequests;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.WithDrawRequests.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.WithDrawRequests.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.WithDrawRequests.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/WithDrawRequests/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/WithDrawRequests/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditWithDrawRequestModal'
        });

		 var _viewWithDrawRequestModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/WithDrawRequests/ViewwithDrawRequestModal',
            modalClass: 'ViewWithDrawRequestModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$withDrawRequestsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _withDrawRequestsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#WithDrawRequestsTableFilter').val(),
					minUserIdFilter: $('#MinUserIdFilterId').val(),
					maxUserIdFilter: $('#MaxUserIdFilterId').val(),
					minAmountFilter: $('#MinAmountFilterId').val(),
					maxAmountFilter: $('#MaxAmountFilterId').val(),
					statusFilter: $('#StatusFilterId').val(),
					minTransactionIdFilter: $('#MinTransactionIdFilterId').val(),
					maxTransactionIdFilter: $('#MaxTransactionIdFilterId').val(),
					minBankAccountIdFilter: $('#MinBankAccountIdFilterId').val(),
					maxBankAccountIdFilter: $('#MaxBankAccountIdFilterId').val()
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
                                    _viewWithDrawRequestModal.open({ id: data.record.withDrawRequest.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.withDrawRequest.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteWithDrawRequest(data.record.withDrawRequest);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "withDrawRequest.userId",
						 name: "userId"   
					},
					{
						targets: 2,
						 data: "withDrawRequest.amount",
						 name: "amount"   
					},
					{
						targets: 3,
						 data: "withDrawRequest.status",
						 name: "status"   ,
						render: function (status) {
							return app.localize('Enum_WithDrawRequestStatus_' + status);
						}
			
					},
					{
						targets: 4,
						 data: "withDrawRequest.transactionId",
						 name: "transactionId"   
					},
					{
						targets: 5,
						 data: "withDrawRequest.bankAccountId",
						 name: "bankAccountId"   
					}
            ]
        });

        function getWithDrawRequests() {
            dataTable.ajax.reload();
        }

        function deleteWithDrawRequest(withDrawRequest) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _withDrawRequestsService.delete({
                            id: withDrawRequest.id
                        }).done(function () {
                            getWithDrawRequests(true);
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

        $('#CreateNewWithDrawRequestButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _withDrawRequestsService
                .getWithDrawRequestsToExcel({
				filter : $('#WithDrawRequestsTableFilter').val(),
					minUserIdFilter: $('#MinUserIdFilterId').val(),
					maxUserIdFilter: $('#MaxUserIdFilterId').val(),
					minAmountFilter: $('#MinAmountFilterId').val(),
					maxAmountFilter: $('#MaxAmountFilterId').val(),
					statusFilter: $('#StatusFilterId').val(),
					minTransactionIdFilter: $('#MinTransactionIdFilterId').val(),
					maxTransactionIdFilter: $('#MaxTransactionIdFilterId').val(),
					minBankAccountIdFilter: $('#MinBankAccountIdFilterId').val(),
					maxBankAccountIdFilter: $('#MaxBankAccountIdFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditWithDrawRequestModalSaved', function () {
            getWithDrawRequests();
        });

		$('#GetWithDrawRequestsButton').click(function (e) {
            e.preventDefault();
            getWithDrawRequests();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getWithDrawRequests();
		  }
		});
    });
})();