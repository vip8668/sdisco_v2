(function ($) {
    app.modals.BlogPostLookupTableModal = function () {

        var _modalManager;

        var _blogProductRelatedsService = abp.services.app.blogProductRelateds;
        var _$blogPostTable = $('#BlogPostTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$blogPostTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _blogProductRelatedsService.getAllBlogPostForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#BlogPostTableFilter').val()
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

        $('#BlogPostTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getBlogPost() {
            dataTable.ajax.reload();
        }

        $('#GetBlogPostButton').click(function (e) {
            e.preventDefault();
            getBlogPost();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getBlogPost();
            }
        });

    };
})(jQuery);

