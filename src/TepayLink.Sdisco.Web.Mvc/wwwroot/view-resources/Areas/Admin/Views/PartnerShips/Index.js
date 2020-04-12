(function () {
    $(function () {

        var _$partnerShipsTable = $('#PartnerShipsTable');
        var _partnerShipsService = abp.services.app.partnerShips;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.PartnerShips.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.PartnerShips.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.PartnerShips.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/PartnerShips/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/PartnerShips/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPartnerShipModal'
        });

		 var _viewPartnerShipModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/PartnerShips/ViewpartnerShipModal',
            modalClass: 'ViewPartnerShipModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$partnerShipsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _partnerShipsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#PartnerShipsTableFilter').val(),
					logoFilter: $('#LogoFilterId').val(),
					titleFilter: $('#TitleFilterId').val(),
					linkFilter: $('#LinkFilterId').val()
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
                                    _viewPartnerShipModal.open({ id: data.record.partnerShip.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.partnerShip.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deletePartnerShip(data.record.partnerShip);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "partnerShip.logo",
						 name: "logo"   
					},
					{
						targets: 2,
						 data: "partnerShip.title",
						 name: "title"   
					},
					{
						targets: 3,
						 data: "partnerShip.link",
						 name: "link"   
					},
					{
						targets: 4,
						 data: "partnerShip.order",
						 name: "order"   
					}
            ]
        });

        function getPartnerShips() {
            dataTable.ajax.reload();
        }

        function deletePartnerShip(partnerShip) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _partnerShipsService.delete({
                            id: partnerShip.id
                        }).done(function () {
                            getPartnerShips(true);
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

        $('#CreateNewPartnerShipButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _partnerShipsService
                .getPartnerShipsToExcel({
				filter : $('#PartnerShipsTableFilter').val(),
					logoFilter: $('#LogoFilterId').val(),
					titleFilter: $('#TitleFilterId').val(),
					linkFilter: $('#LinkFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPartnerShipModalSaved', function () {
            getPartnerShips();
        });

		$('#GetPartnerShipsButton').click(function (e) {
            e.preventDefault();
            getPartnerShips();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getPartnerShips();
		  }
		});
    });
})();