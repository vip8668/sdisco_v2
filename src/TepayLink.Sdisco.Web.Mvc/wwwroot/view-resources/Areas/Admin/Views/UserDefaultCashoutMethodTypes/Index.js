(function () {
    $(function () {

        var _$userDefaultCashoutMethodTypesTable = $('#UserDefaultCashoutMethodTypesTable');
        var _userDefaultCashoutMethodTypesService = abp.services.app.userDefaultCashoutMethodTypes;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.UserDefaultCashoutMethodTypes.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.UserDefaultCashoutMethodTypes.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.UserDefaultCashoutMethodTypes.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/UserDefaultCashoutMethodTypes/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/UserDefaultCashoutMethodTypes/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditUserDefaultCashoutMethodTypeModal'
        });

		 var _viewUserDefaultCashoutMethodTypeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/UserDefaultCashoutMethodTypes/ViewuserDefaultCashoutMethodTypeModal',
            modalClass: 'ViewUserDefaultCashoutMethodTypeModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$userDefaultCashoutMethodTypesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _userDefaultCashoutMethodTypesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#UserDefaultCashoutMethodTypesTableFilter').val(),
					cashoutMethodTypeTitleFilter: $('#CashoutMethodTypeTitleFilterId').val(),
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
                                    _viewUserDefaultCashoutMethodTypeModal.open({ id: data.record.userDefaultCashoutMethodType.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.userDefaultCashoutMethodType.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteUserDefaultCashoutMethodType(data.record.userDefaultCashoutMethodType);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "cashoutMethodTypeTitle" ,
						 name: "cashoutMethodTypeFk.title" 
					},
					{
						targets: 2,
						 data: "userName" ,
						 name: "userFk.name" 
					}
            ]
        });

        function getUserDefaultCashoutMethodTypes() {
            dataTable.ajax.reload();
        }

        function deleteUserDefaultCashoutMethodType(userDefaultCashoutMethodType) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _userDefaultCashoutMethodTypesService.delete({
                            id: userDefaultCashoutMethodType.id
                        }).done(function () {
                            getUserDefaultCashoutMethodTypes(true);
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

        $('#CreateNewUserDefaultCashoutMethodTypeButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _userDefaultCashoutMethodTypesService
                .getUserDefaultCashoutMethodTypesToExcel({
				filter : $('#UserDefaultCashoutMethodTypesTableFilter').val(),
					cashoutMethodTypeTitleFilter: $('#CashoutMethodTypeTitleFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditUserDefaultCashoutMethodTypeModalSaved', function () {
            getUserDefaultCashoutMethodTypes();
        });

		$('#GetUserDefaultCashoutMethodTypesButton').click(function (e) {
            e.preventDefault();
            getUserDefaultCashoutMethodTypes();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getUserDefaultCashoutMethodTypes();
		  }
		});
    });
})();