(function () {
    $(function () {

        var _$partnersTable = $('#PartnersTable');
        var _partnersService = abp.services.app.partners;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Partners.Create'),
            edit: abp.auth.hasPermission('Pages.Partners.Edit'),
            'delete': abp.auth.hasPermission('Pages.Partners.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Partners/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Partners/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPartnerModal'
        });

		 var _viewPartnerModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Partners/ViewpartnerModal',
            modalClass: 'ViewPartnerModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$partnersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _partnersService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#PartnersTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					websiteUrlFilter: $('#WebsiteUrlFilterId').val(),
					languagesFilter: $('#LanguagesFilterId').val(),
					skypeIdFilter: $('#SkypeIdFilterId').val(),
					commentFilter: $('#CommentFilterId').val(),
					affiliateKeyFilter: $('#AffiliateKeyFilterId').val(),
					minStatusFilter: $('#MinStatusFilterId').val(),
					maxStatusFilter: $('#MaxStatusFilterId').val(),
					alreadyBecomeSdiscoPartnerFilter: $('#AlreadyBecomeSdiscoPartnerFilterId').val(),
					hasDriverLicenseFilter: $('#HasDriverLicenseFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val(),
					detinationNameFilter: $('#DetinationNameFilterId').val()
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
                                    _viewPartnerModal.open({ id: data.record.partner.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.partner.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deletePartner(data.record.partner);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "partner.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "partner.websiteUrl",
						 name: "websiteUrl"   
					},
					{
						targets: 3,
						 data: "partner.languages",
						 name: "languages"   
					},
					{
						targets: 4,
						 data: "partner.skypeId",
						 name: "skypeId"   
					},
					{
						targets: 5,
						 data: "partner.comment",
						 name: "comment"   
					},
					{
						targets: 6,
						 data: "partner.affiliateKey",
						 name: "affiliateKey"   
					},
					{
						targets: 7,
						 data: "partner.status",
						 name: "status"   
					},
					{
						targets: 8,
						 data: "partner.alreadyBecomeSdiscoPartner",
						 name: "alreadyBecomeSdiscoPartner"  ,
						render: function (alreadyBecomeSdiscoPartner) {
							if (alreadyBecomeSdiscoPartner) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 9,
						 data: "partner.hasDriverLicense",
						 name: "hasDriverLicense"  ,
						render: function (hasDriverLicense) {
							if (hasDriverLicense) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 10,
						 data: "userName" ,
						 name: "userFk.name" 
					},
					{
						targets: 11,
						 data: "detinationName" ,
						 name: "detinationFk.name" 
					}
            ]
        });

        function getPartners() {
            dataTable.ajax.reload();
        }

        function deletePartner(partner) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _partnersService.delete({
                            id: partner.id
                        }).done(function () {
                            getPartners(true);
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

        $('#CreateNewPartnerButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _partnersService
                .getPartnersToExcel({
				filter : $('#PartnersTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					websiteUrlFilter: $('#WebsiteUrlFilterId').val(),
					languagesFilter: $('#LanguagesFilterId').val(),
					skypeIdFilter: $('#SkypeIdFilterId').val(),
					commentFilter: $('#CommentFilterId').val(),
					affiliateKeyFilter: $('#AffiliateKeyFilterId').val(),
					minStatusFilter: $('#MinStatusFilterId').val(),
					maxStatusFilter: $('#MaxStatusFilterId').val(),
					alreadyBecomeSdiscoPartnerFilter: $('#AlreadyBecomeSdiscoPartnerFilterId').val(),
					hasDriverLicenseFilter: $('#HasDriverLicenseFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val(),
					detinationNameFilter: $('#DetinationNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPartnerModalSaved', function () {
            getPartners();
        });

		$('#GetPartnersButton').click(function (e) {
            e.preventDefault();
            getPartners();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getPartners();
		  }
		});
    });
})();