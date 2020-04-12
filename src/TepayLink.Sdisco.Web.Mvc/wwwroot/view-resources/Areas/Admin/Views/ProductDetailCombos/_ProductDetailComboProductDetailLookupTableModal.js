(function ($) {
    app.modals.ProductDetailLookupTableModal = function () {

        var _modalManager;

        var _productDetailCombosService = abp.services.app.productDetailCombos;
        var _$productDetailTable = $('#ProductDetailTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$productDetailTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _productDetailCombosService.getAllProductDetailForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#ProductDetailTableFilter').val()
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

        $('#ProductDetailTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getProductDetail() {
            dataTable.ajax.reload();
        }

        $('#GetProductDetailButton').click(function (e) {
            e.preventDefault();
            getProductDetail();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getProductDetail();
            }
        });

    };
})(jQuery);

