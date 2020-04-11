(function ($) {
    app.modals.HelpCategoryLookupTableModal = function () {

        var _modalManager;

        var _helpContentsService = abp.services.app.helpContents;
        var _$helpCategoryTable = $('#HelpCategoryTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$helpCategoryTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _helpContentsService.getAllHelpCategoryForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#HelpCategoryTableFilter').val()
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

        $('#HelpCategoryTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getHelpCategory() {
            dataTable.ajax.reload();
        }

        $('#GetHelpCategoryButton').click(function (e) {
            e.preventDefault();
            getHelpCategory();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getHelpCategory();
            }
        });

    };
})(jQuery);

