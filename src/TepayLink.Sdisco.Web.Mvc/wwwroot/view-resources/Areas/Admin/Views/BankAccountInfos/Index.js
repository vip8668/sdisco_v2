(function () {
    $(function () {

        var _$bankAccountInfosTable = $('#BankAccountInfosTable');
        var _bankAccountInfosService = abp.services.app.bankAccountInfos;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.BankAccountInfos.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.BankAccountInfos.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.BankAccountInfos.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BankAccountInfos/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/BankAccountInfos/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditBankAccountInfoModal'
        });

		 var _viewBankAccountInfoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BankAccountInfos/ViewbankAccountInfoModal',
            modalClass: 'ViewBankAccountInfoModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$bankAccountInfosTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _bankAccountInfosService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#BankAccountInfosTableFilter').val(),
					accountNameFilter: $('#AccountNameFilterId').val(),
					accountNoFilter: $('#AccountNoFilterId').val(),
					bankBankNameFilter: $('#BankBankNameFilterId').val(),
					bankBranchBranchNameFilter: $('#BankBranchBranchNameFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val()
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
                                    _viewBankAccountInfoModal.open({ id: data.record.bankAccountInfo.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.bankAccountInfo.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteBankAccountInfo(data.record.bankAccountInfo);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "bankAccountInfo.accountName",
						 name: "accountName"   
					},
					{
						targets: 2,
						 data: "bankAccountInfo.accountNo",
						 name: "accountNo"   
					},
					{
						targets: 3,
						 data: "bankBankName" ,
						 name: "bankFk.bankName" 
					},
					{
						targets: 4,
						 data: "bankBranchBranchName" ,
						 name: "bankBranchFk.branchName" 
					},
					{
						targets: 5,
						 data: "userName" ,
						 name: "userFk.name" 
					}
            ]
        });

        function getBankAccountInfos() {
            dataTable.ajax.reload();
        }

        function deleteBankAccountInfo(bankAccountInfo) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _bankAccountInfosService.delete({
                            id: bankAccountInfo.id
                        }).done(function () {
                            getBankAccountInfos(true);
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

        $('#CreateNewBankAccountInfoButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _bankAccountInfosService
                .getBankAccountInfosToExcel({
				filter : $('#BankAccountInfosTableFilter').val(),
					accountNameFilter: $('#AccountNameFilterId').val(),
					accountNoFilter: $('#AccountNoFilterId').val(),
					bankBankNameFilter: $('#BankBankNameFilterId').val(),
					bankBranchBranchNameFilter: $('#BankBranchBranchNameFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditBankAccountInfoModalSaved', function () {
            getBankAccountInfos();
        });

		$('#GetBankAccountInfosButton').click(function (e) {
            e.preventDefault();
            getBankAccountInfos();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getBankAccountInfos();
		  }
		});
    });
})();