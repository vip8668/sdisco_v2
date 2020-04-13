(function () {
    $(function () {

        var _$shortLinksTable = $('#ShortLinksTable');
        var _shortLinksService = abp.services.app.shortLinks;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.ShortLinks.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.ShortLinks.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.ShortLinks.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ShortLinks/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ShortLinks/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditShortLinkModal'
        });

		 var _viewShortLinkModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ShortLinks/ViewshortLinkModal',
            modalClass: 'ViewShortLinkModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$shortLinksTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _shortLinksService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ShortLinksTableFilter').val()
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
                                    _viewShortLinkModal.open({ id: data.record.shortLink.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.shortLink.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteShortLink(data.record.shortLink);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "shortLink.userId",
						 name: "userId"   
					},
					{
						targets: 2,
						 data: "shortLink.fullLink",
						 name: "fullLink"   
					},
					{
						targets: 3,
						 data: "shortLink.shortCode",
						 name: "shortCode"   
					}
            ]
        });

        function getShortLinks() {
            dataTable.ajax.reload();
        }

        function deleteShortLink(shortLink) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _shortLinksService.delete({
                            id: shortLink.id
                        }).done(function () {
                            getShortLinks(true);
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

        $('#CreateNewShortLinkButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _shortLinksService
                .getShortLinksToExcel({
				filter : $('#ShortLinksTableFilter').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditShortLinkModalSaved', function () {
            getShortLinks();
        });

		$('#GetShortLinksButton').click(function (e) {
            e.preventDefault();
            getShortLinks();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getShortLinks();
		  }
		});
    });
})();