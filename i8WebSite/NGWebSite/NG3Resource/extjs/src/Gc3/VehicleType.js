//车辆类别帮助gcVehicleTypeRichHelp
Ext.define('Ext.Gc3.VehicleType', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcVehicleTypeRichHelp',
    helpid: 'pmm3.pmw3_vehicle_type',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Name',
    //listFields: '',
    //listHeadTexts: '',
    MaxLength: 100,
    editable: false,
    mustInput: false,
    colspan: 1
});