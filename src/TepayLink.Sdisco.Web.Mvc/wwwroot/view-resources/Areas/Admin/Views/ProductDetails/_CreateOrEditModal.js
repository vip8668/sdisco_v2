(function ($) {
    app.modals.CreateOrEditProductDetailModal = function () {

        var _productDetailsService = abp.services.app.productDetails;

        var _modalManager;
        var _$productDetailInformationForm = null;

		        var _ProductDetailproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductDetails/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ProductDetails/_ProductDetailProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$productDetailInformationForm = _modalManager.getModal().find('form[name=ProductDetailInformationsForm]');
            _$productDetailInformationForm.validate();
        };

		          $('#OpenProductLookupTableButton').click(function () {

            var productDetail = _$productDetailInformationForm.serializeFormToObject();

            _ProductDetailproductLookupTableModal.open({ id: productDetail.productId, displayName: productDetail.productName }, function (data) {
                _$productDetailInformationForm.find('input[name=productName]').val(data.displayName); 
                _$productDetailInformationForm.find('input[name=productId]').val(data.id); 
            });
        });
		
		$('#ClearProductNameButton').click(function () {
                _$productDetailInformationForm.find('input[name=productName]').val(''); 
                _$productDetailInformationForm.find('input[name=productId]').val(''); 
        });
		


        this.save = function () {
            if (!_$productDetailInformationForm.valid()) {
                return;
            }
            if ($('#ProductDetail_ProductId').prop('required') && $('#ProductDetail_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }

            var productDetail = _$productDetailInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _productDetailsService.createOrEdit(
				productDetail
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditProductDetailModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);