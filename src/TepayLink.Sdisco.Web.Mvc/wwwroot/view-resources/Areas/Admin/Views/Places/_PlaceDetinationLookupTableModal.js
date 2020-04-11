(function ($) {
    app.modals.DetinationLookupTableModal = function () {

        var _modalManager;

        var _placesService = abp.services.app.places;
        var _$detinationTable = $('#DetinationTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$detinationTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _placesService.getAllDetinationForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#DetinationTableFilter').val()
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

        $('#DetinationTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getDetination() {
            dataTable.ajax.reload();
        }

        $('#GetDetinationButton').click(function (e) {
            e.preventDefault();
            getDetination();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDetination();
            }
        });

    };
})(jQuery);

