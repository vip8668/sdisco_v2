(function ($) {
    app.modals.CashoutMethodTypeLookupTableModal = function () {

        var _modalManager;

        var _userDefaultCashoutMethodTypesService = abp.services.app.userDefaultCashoutMethodTypes;
        var _$cashoutMethodTypeTable = $('#CashoutMethodTypeTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$cashoutMethodTypeTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _userDefaultCashoutMethodTypesService.getAllCashoutMethodTypeForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#CashoutMethodTypeTableFilter').val()
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

        $('#CashoutMethodTypeTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getCashoutMethodType() {
            dataTable.ajax.reload();
        }

        $('#GetCashoutMethodTypeButton').click(function (e) {
            e.preventDefault();
            getCashoutMethodType();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getCashoutMethodType();
            }
        });

    };
})(jQuery);

