(function ($) {
    app.modals.BankBranchLookupTableModal = function () {

        var _modalManager;

        var _bankAccountInfosService = abp.services.app.bankAccountInfos;
        var _$bankBranchTable = $('#BankBranchTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$bankBranchTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _bankAccountInfosService.getAllBankBranchForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#BankBranchTableFilter').val()
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

        $('#BankBranchTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getBankBranch() {
            dataTable.ajax.reload();
        }

        $('#GetBankBranchButton').click(function (e) {
            e.preventDefault();
            getBankBranch();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getBankBranch();
            }
        });

    };
})(jQuery);

