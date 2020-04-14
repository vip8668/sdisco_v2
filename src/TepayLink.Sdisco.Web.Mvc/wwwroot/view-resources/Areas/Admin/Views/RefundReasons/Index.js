(function () {
    $(function () {

        var _$refundReasonsTable = $('#RefundReasonsTable');
        var _refundReasonsService = abp.services.app.refundReasons;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.RefundReasons.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.RefundReasons.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.RefundReasons.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/RefundReasons/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/RefundReasons/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditRefundReasonModal'
        });

		 var _viewRefundReasonModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/RefundReasons/ViewrefundReasonModal',
            modalClass: 'ViewRefundReasonModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$refundReasonsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _refundReasonsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#RefundReasonsTableFilter').val(),
					reasonTextFilter: $('#ReasonTextFilterId').val()
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
                                    _viewRefundReasonModal.open({ id: data.record.refundReason.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.refundReason.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteRefundReason(data.record.refundReason);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "refundReason.reasonText",
						 name: "reasonText"   
					}
            ]
        });

        function getRefundReasons() {
            dataTable.ajax.reload();
        }

        function deleteRefundReason(refundReason) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _refundReasonsService.delete({
                            id: refundReason.id
                        }).done(function () {
                            getRefundReasons(true);
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

        $('#CreateNewRefundReasonButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _refundReasonsService
                .getRefundReasonsToExcel({
				filter : $('#RefundReasonsTableFilter').val(),
					reasonTextFilter: $('#ReasonTextFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditRefundReasonModalSaved', function () {
            getRefundReasons();
        });

		$('#GetRefundReasonsButton').click(function (e) {
            e.preventDefault();
            getRefundReasons();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getRefundReasons();
		  }
		});
    });
})();