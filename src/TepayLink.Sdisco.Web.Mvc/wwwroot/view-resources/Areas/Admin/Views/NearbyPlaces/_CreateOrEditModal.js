(function ($) {
    app.modals.CreateOrEditNearbyPlaceModal = function () {

        var _nearbyPlacesService = abp.services.app.nearbyPlaces;

        var _modalManager;
        var _$nearbyPlaceInformationForm = null;

		        var _NearbyPlaceplaceLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/NearbyPlaces/PlaceLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/NearbyPlaces/_NearbyPlacePlaceLookupTableModal.js',
            modalClass: 'PlaceLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$nearbyPlaceInformationForm = _modalManager.getModal().find('form[name=NearbyPlaceInformationsForm]');
            _$nearbyPlaceInformationForm.validate();
        };

		          $('#OpenPlaceLookupTableButton').click(function () {

            var nearbyPlace = _$nearbyPlaceInformationForm.serializeFormToObject();

            _NearbyPlaceplaceLookupTableModal.open({ id: nearbyPlace.placeId, displayName: nearbyPlace.placeName }, function (data) {
                _$nearbyPlaceInformationForm.find('input[name=placeName]').val(data.displayName); 
                _$nearbyPlaceInformationForm.find('input[name=placeId]').val(data.id); 
            });
        });
		
		$('#ClearPlaceNameButton').click(function () {
                _$nearbyPlaceInformationForm.find('input[name=placeName]').val(''); 
                _$nearbyPlaceInformationForm.find('input[name=placeId]').val(''); 
        });
		
        $('#OpenPlace2LookupTableButton').click(function () {

            var nearbyPlace = _$nearbyPlaceInformationForm.serializeFormToObject();

            _NearbyPlaceplaceLookupTableModal.open({ id: nearbyPlace.nearbyPlaceId, displayName: nearbyPlace.placeName2 }, function (data) {
                _$nearbyPlaceInformationForm.find('input[name=placeName2]').val(data.displayName); 
                _$nearbyPlaceInformationForm.find('input[name=nearbyPlaceId]').val(data.id); 
            });
        });
		
		$('#ClearPlaceName2Button').click(function () {
                _$nearbyPlaceInformationForm.find('input[name=placeName2]').val(''); 
                _$nearbyPlaceInformationForm.find('input[name=nearbyPlaceId]').val(''); 
        });
		


        this.save = function () {
            if (!_$nearbyPlaceInformationForm.valid()) {
                return;
            }
            if ($('#NearbyPlace_PlaceId').prop('required') && $('#NearbyPlace_PlaceId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Place')));
                return;
            }
            if ($('#NearbyPlace_NearbyPlaceId').prop('required') && $('#NearbyPlace_NearbyPlaceId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Place')));
                return;
            }

            var nearbyPlace = _$nearbyPlaceInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _nearbyPlacesService.createOrEdit(
				nearbyPlace
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditNearbyPlaceModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);