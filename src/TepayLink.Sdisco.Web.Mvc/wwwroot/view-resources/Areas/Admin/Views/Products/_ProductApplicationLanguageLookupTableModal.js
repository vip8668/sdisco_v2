(function ($) {
    app.modals.ApplicationLanguageLookupTableModal = function () {

        var _modalManager;

        var _productsService = abp.services.app.products;
        var _$applicationLanguageTable = $('#ApplicationLanguageTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$applicationLanguageTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _productsService.getAllApplicationLanguageForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#ApplicationLanguageTableFilter').val()
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

        $('#ApplicationLanguageTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getApplicationLanguage() {
            dataTable.ajax.reload();
        }

        $('#GetApplicationLanguageButton').click(function (e) {
            e.preventDefault();
            getApplicationLanguage();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getApplicationLanguage();
            }
        });

    };
})(jQuery);

