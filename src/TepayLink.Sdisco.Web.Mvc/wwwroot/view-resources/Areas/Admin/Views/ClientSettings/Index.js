(function () {
    $(function () {

        var _$clientSettingsTable = $('#ClientSettingsTable');
        var _clientSettingsService = abp.services.app.clientSettings;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.ClientSettings.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.ClientSettings.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.ClientSettings.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ClientSettings/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ClientSettings/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditClientSettingModal'
        });

		 var _viewClientSettingModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ClientSettings/ViewclientSettingModal',
            modalClass: 'ViewClientSettingModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$clientSettingsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _clientSettingsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ClientSettingsTableFilter').val(),
					keyFilter: $('#KeyFilterId').val(),
					valueFilter: $('#ValueFilterId').val()
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
                                    _viewClientSettingModal.open({ id: data.record.clientSetting.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.clientSetting.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteClientSetting(data.record.clientSetting);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "clientSetting.key",
						 name: "key"   
					},
					{
						targets: 2,
						 data: "clientSetting.value",
						 name: "value"   
					}
            ]
        });

        function getClientSettings() {
            dataTable.ajax.reload();
        }

        function deleteClientSetting(clientSetting) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _clientSettingsService.delete({
                            id: clientSetting.id
                        }).done(function () {
                            getClientSettings(true);
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

        $('#CreateNewClientSettingButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _clientSettingsService
                .getClientSettingsToExcel({
				filter : $('#ClientSettingsTableFilter').val(),
					keyFilter: $('#KeyFilterId').val(),
					valueFilter: $('#ValueFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditClientSettingModalSaved', function () {
            getClientSettings();
        });

		$('#GetClientSettingsButton').click(function (e) {
            e.preventDefault();
            getClientSettings();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getClientSettings();
		  }
		});
    });
})();