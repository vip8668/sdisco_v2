(function () {
    $(function () {

        var _$userSubcribersTable = $('#UserSubcribersTable');
        var _userSubcribersService = abp.services.app.userSubcribers;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.UserSubcribers.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.UserSubcribers.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.UserSubcribers.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/UserSubcribers/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/UserSubcribers/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditUserSubcriberModal'
        });

		 var _viewUserSubcriberModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/UserSubcribers/ViewuserSubcriberModal',
            modalClass: 'ViewUserSubcriberModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$userSubcribersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _userSubcribersService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#UserSubcribersTableFilter').val(),
					emailFilter: $('#EmailFilterId').val()
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
                                    _viewUserSubcriberModal.open({ id: data.record.userSubcriber.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.userSubcriber.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteUserSubcriber(data.record.userSubcriber);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "userSubcriber.email",
						 name: "email"   
					}
            ]
        });

        function getUserSubcribers() {
            dataTable.ajax.reload();
        }

        function deleteUserSubcriber(userSubcriber) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _userSubcribersService.delete({
                            id: userSubcriber.id
                        }).done(function () {
                            getUserSubcribers(true);
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

        $('#CreateNewUserSubcriberButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _userSubcribersService
                .getUserSubcribersToExcel({
				filter : $('#UserSubcribersTableFilter').val(),
					emailFilter: $('#EmailFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditUserSubcriberModalSaved', function () {
            getUserSubcribers();
        });

		$('#GetUserSubcribersButton').click(function (e) {
            e.preventDefault();
            getUserSubcribers();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getUserSubcribers();
		  }
		});
    });
})();