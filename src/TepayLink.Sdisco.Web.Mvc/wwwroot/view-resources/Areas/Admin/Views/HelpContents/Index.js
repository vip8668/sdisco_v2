(function () {
    $(function () {

        var _$helpContentsTable = $('#HelpContentsTable');
        var _helpContentsService = abp.services.app.helpContents;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.HelpContents.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.HelpContents.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.HelpContents.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/HelpContents/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/HelpContents/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditHelpContentModal'
        });

		 var _viewHelpContentModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/HelpContents/ViewhelpContentModal',
            modalClass: 'ViewHelpContentModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$helpContentsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _helpContentsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#HelpContentsTableFilter').val(),
					helpCategoryCategoryNameFilter: $('#HelpCategoryCategoryNameFilterId').val()
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
                                    _viewHelpContentModal.open({ id: data.record.helpContent.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.helpContent.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteHelpContent(data.record.helpContent);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "helpContent.question",
						 name: "question"   
					},
					{
						targets: 2,
						 data: "helpContent.answer",
						 name: "answer"   
					},
					{
						targets: 3,
						 data: "helpCategoryCategoryName" ,
						 name: "helpCategoryFk.categoryName" 
					}
            ]
        });

        function getHelpContents() {
            dataTable.ajax.reload();
        }

        function deleteHelpContent(helpContent) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _helpContentsService.delete({
                            id: helpContent.id
                        }).done(function () {
                            getHelpContents(true);
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

        $('#CreateNewHelpContentButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _helpContentsService
                .getHelpContentsToExcel({
				filter : $('#HelpContentsTableFilter').val(),
					helpCategoryCategoryNameFilter: $('#HelpCategoryCategoryNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditHelpContentModalSaved', function () {
            getHelpContents();
        });

		$('#GetHelpContentsButton').click(function (e) {
            e.preventDefault();
            getHelpContents();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getHelpContents();
		  }
		});
    });
})();