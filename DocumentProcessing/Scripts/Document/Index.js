"use strict";
var documentPageVueInstance;


document.addEventListener("DOMContentLoaded", function (event) {
    Vue.use(VuelidateErrorExtractor.default, {
        template: VuelidateErrorExtractor.templates.foundation6
    });

    documentPageVueInstance = new Vue({
        el: '#documentPage',
        data: () => ({
            
            form: {                
                Id: null,
                Name: null,
                Path: null,
                CreatorId: null,
                RegistrationDate: new Date(),
                Created: new Date(),
                Creator: null,
                Type: null,
                AllTypes: null,
                ExecutionPeriod: new Date(),
                NomenclatureId: null,
                DocIndex: null,
                DocHeader: null,
                ManagerId: null,
                Resolution: null,
                ExecutorId: null,
                ExecutorNote: null,
                ControllerId: null,
                ControllerNote: null,
                DocumentFile: null,
                Managers: null,
                AllNomenclature: null
                
            },
            editing: true,
            sending: false,
            lastUser: null
        }),
       
        //computed: {
        //    orderedTypes: function () {
        //        return this.AllTypes
        //    }
        //    //creationDate: function () {
        //    //    return new Date()
        //    //},
        //    //regDate: function () {
        //    //    return new Date()
        //    //},
        //    //expirationDate: function () {
        //    //    return new Date()
        //    //},
        //},

        mounted: function () {
            this.editing = !window.model.IsDocumentExists;
            this.form = window.model.DocumentModel;
            this.form.DocumentFile = null;
        },

        validations: {
            form: {
                Name: {
                    required: validators.required,
                    minLength: validators.minLength(5)
                },
                Created: {
                    required: validators.required
                },
                ExecutionPeriod: {
                    required: validators.required
                },
                NomenclatureId: {
                    required: validators.required
                },
                Type: {
                    required: validators.required,
                    between: validators.between(1, 99999999)
                },
                ManagerId: {
                    required: validators.required,
                    between: validators.between(1, 99999999)
                },
                DocumentFile: {
                    required: validators.required
                }
            }
        },
        //computed: {
        //    orderedTypes: function () {
        //        return _.orderBy(this.AllTypes, 'Name')
        //    },

        methods: {
            getValidationClass(fieldName) {
                const field = this.$v.form[fieldName]

                if (field) {
                    return {
                        'md-invalid': field.$invalid && field.$dirty
                    }
                }
            },
            orderedTypes: function () {
                return _.orderBy(this.AllTypes, 'Name')
            },

            

            saveDocument() {
                this.sending = true;
                var _this = this;
                var regDate = new Date();
                var formData = new FormData(document.getElementById('documentForm'));

                formData.append("Id", this.form.Id);
                formData.append("Name", this.form.Name);
                formData.append("Path", this.form.DocumentFile);                  
                formData.append("CreatorId", this.form.CreatorId);
                formData.append("Created", this.form.Created);
                formData.append("Type", this.form.Type);
                formData.append("ExecutionPeriod", this.form.ExecutionPeriod);
                formData.append("RegistrationDate", regDate.toLocaleDateString());
                formData.append("ManagerId", this.form.ManagerId);
                formData.append("NomenclatureId", this.form.NomenclatureId);

                Vue.http.post('/Document/Index', formData)
                    .then(response => {
                        //success callback
                        _this.documentCreated = true;
                        this.lastUser = `${this.form.firstName} ${this.form.lastName}`;
                        this.sending = false;
                        if (this.userSaved) {
                            document.location.href = "/";
                        }
                    }, response => {
                        // error callback
                        _this.documentCreated = false;
                        this.sending = false;
                    });
            },
            validateDocument() {
                this.$v.$touch();

                if (!this.$v.$invalid) {
                    this.saveDocument();
                }
            }
        }

    });
});