(function ($) {
    app.modals.CreateOrEditPlaceModal = function () {

        var _placesService = abp.services.app.places;

        var _modalManager;
        var _$placeInformationForm = null;

		        var _PlacedetinationLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Places/DetinationLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Places/_PlaceDetinationLookupTableModal.js',
            modalClass: 'DetinationLookupTableModal'
        });        var _PlaceplaceCategoryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Places/PlaceCategoryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Places/_PlacePlaceCategoryLookupTableModal.js',
            modalClass: 'PlaceCategoryLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$placeInformationForm = _modalManager.getModal().find('form[name=PlaceInformationsForm]');
            _$placeInformationForm.validate();
        };

		          $('#OpenDetinationLookupTableButton').click(function () {

            var place = _$placeInformationForm.serializeFormToObject();

            _PlacedetinationLookupTableModal.open({ id: place.detinationId, displayName: place.detinationName }, function (data) {
                _$placeInformationForm.find('input[name=detinationName]').val(data.displayName); 
                _$placeInformationForm.find('input[name=detinationId]').val(data.id); 
            });
        });
		
		$('#ClearDetinationNameButton').click(function () {
                _$placeInformationForm.find('input[name=detinationName]').val(''); 
                _$placeInformationForm.find('input[name=detinationId]').val(''); 
        });
		
        $('#OpenPlaceCategoryLookupTableButton').click(function () {

            var place = _$placeInformationForm.serializeFormToObject();

            _PlaceplaceCategoryLookupTableModal.open({ id: place.placeCategoryId, displayName: place.placeCategoryName }, function (data) {
                _$placeInformationForm.find('input[name=placeCategoryName]').val(data.displayName); 
                _$placeInformationForm.find('input[name=placeCategoryId]').val(data.id); 
            });
        });
		
		$('#ClearPlaceCategoryNameButton').click(function () {
                _$placeInformationForm.find('input[name=placeCategoryName]').val(''); 
                _$placeInformationForm.find('input[name=placeCategoryId]').val(''); 
        });
		


        this.save = function () {
            if (!_$placeInformationForm.valid()) {
                return;
            }
            if ($('#Place_DetinationId').prop('required') && $('#Place_DetinationId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Detination')));
                return;
            }
            if ($('#Place_PlaceCategoryId').prop('required') && $('#Place_PlaceCategoryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('PlaceCategory')));
                return;
            }

            var place = _$placeInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _placesService.createOrEdit(
				place
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditPlaceModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);