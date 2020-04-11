(function ($) {
    app.modals.BankLookupTableModal = function () {

        var _modalManager;

        var _bankAccountInfosService = abp.services.app.bankAccountInfos;
        var _$bankTable = $('#BankTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$bankTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _bankAccountInfosService.getAllBankForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#BankTableFilter').val()
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

        $('#BankTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getBank() {
            dataTable.ajax.reload();
        }

        $('#GetBankButton').click(function (e) {
            e.preventDefault();
            getBank();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getBank();
            }
        });

    };
})(jQuery);

