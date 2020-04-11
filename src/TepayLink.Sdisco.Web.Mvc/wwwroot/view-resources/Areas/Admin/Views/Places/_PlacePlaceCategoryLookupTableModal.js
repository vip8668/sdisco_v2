(function ($) {
    app.modals.PlaceCategoryLookupTableModal = function () {

        var _modalManager;

        var _placesService = abp.services.app.places;
        var _$placeCategoryTable = $('#PlaceCategoryTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$placeCategoryTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _placesService.getAllPlaceCategoryForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#PlaceCategoryTableFilter').val()
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

        $('#PlaceCategoryTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getPlaceCategory() {
            dataTable.ajax.reload();
        }

        $('#GetPlaceCategoryButton').click(function (e) {
            e.preventDefault();
            getPlaceCategory();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getPlaceCategory();
            }
        });

    };
})(jQuery);

