(function ($) {
    app.modals.ClaimReasonLookupTableModal = function () {

        var _modalManager;

        var _bookingClaimsService = abp.services.app.bookingClaims;
        var _$claimReasonTable = $('#ClaimReasonTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$claimReasonTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _bookingClaimsService.getAllClaimReasonForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#ClaimReasonTableFilter').val()
                    };
                }
            },
            columnDefs: [
                {
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: "<div class=\"text-center\"><input id='selectbtn' class='btn btn-success' type='button' width='25px' value='" + app.localize('Select') + "' /></div>"
                },
                {
                    autoWidth: false,
                    orderable: false,
                    targets: 1,
                    data: "displayName"
                }
            ]
        });

        $('#ClaimReasonTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getClaimReason() {
            dataTable.ajax.reload();
        }

        $('#GetClaimReasonButton').click(function (e) {
            e.preventDefault();
            getClaimReason();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getClaimReason();
            }
        });

    };
})(jQuery);

