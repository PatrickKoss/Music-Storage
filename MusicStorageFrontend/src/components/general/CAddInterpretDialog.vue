<template>
  <v-layout id="root">
    <v-dialog
            :dark="darkeningGeneral"
            max-width="500"
            v-model="dialogD"
            :persistent="true"
    >
      <v-card :dark="darkeningGeneral">
        <v-card-title class="headline" primary-title
        >Add new interpret</v-card-title
        >

        <v-card-text>
          <form ref="form">
            <v-layout row wrap>
              <v-text-field
                      v-model="interpret.Name"
                      label="Interpret"
                      ref="title"
                      :error-messages="errorsName"
              />
            </v-layout>
          </form>
        </v-card-text>

        <v-divider/>

        <v-card-actions>
          <v-btn @click="cancel()" flat>Cancel</v-btn>
          <v-spacer/>
          <v-btn @click="addMusicFile()" color="primary" flat v-if="!editMode"
          >Add interpret
          </v-btn>
          <v-btn @click="addMusicFile()" color="primary" flat v-if="editMode"
          >Edit interpret
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-layout>
</template>

<script lang="ts">
  import Vue from "vue";
  import Component from "vue-class-component";
  import { Prop, Watch } from "vue-property-decorator";
  import { StateModule, AppStore } from "../../store/AppStore";
  import { VueStateField } from "../../store/State";
  import { IMusicFileWithoutIDs } from "../../model/IMusicFileWithoutIDs";
  import { MusicFileRestClient } from "../../model/MusicFileRestClient";
  import { IMusicFile } from "../../model/IMusicFile";
  import HelperFunctions from "../../model/HelperFunctions";
  import IInterpret from "../../model/IInterpret";

  @Component({
    components: {}
  })
  export default class CAddInterpretDialog extends Vue {
    @VueStateField(StateModule.GENERAL)
    public darkeningGeneral: boolean;

    @VueStateField(StateModule.GENERAL)
    public timeout: number;

    @Prop() public dialog: boolean;
    @Prop() public editMode: boolean;
    @Prop() public interpretProp: IInterpret;

    firstValidate = false;

    appStore = AppStore;
    helper: any = new HelperFunctions();

    public dialogD: boolean = this.dialog;
    errorsName = [];

    $refs!: {
      title: any;
      form: any;
    };

    interpret = {
      Name: "",
      ID: -1
    } as IInterpret;

    async addMusicFile() {
      this.firstValidate = true;
      let validation = true;
      if (this.interpret.Name.length === 0) {
        this.errorsName.push("This field is required.");
        validation = false;
      }
      if (validation) {
        try {
          if (this.editMode) {
            let interpret = this.interpretProp;
            interpret.Name = this.interpret.Name;
            const text = (await MusicFileRestClient.updateInterpret(interpret))
              .data;
            this.helper.showError(text, this.timeout, false);
          } else {
            let interpret = this.interpretProp;
            interpret.Name = this.interpret.Name;
            interpret.ID = -1;
            const text = (await MusicFileRestClient.createInterpret(interpret))
              .data;
            this.helper.showError(text, this.timeout, false);
          }
          AppStore.commit("loadInterprets");
        } catch (e) {
          console.log(e);
        }
        this.dialogD = false;
        this.firstValidate = false;
      }
    }

    cancel() {
      this.dialogD = false;
      this.firstValidate = false;
    }

    /**
     * to avoid warnings in the console, a component intern dialog variable needs to be set like the property
     * @private
     */
    @Watch("dialog")
    public __dialog() {
      this.dialogD = this.dialog;
    }
    /**
     * once the dialogD is false, the dialog in the parent should also be false, so message has to be sent to tbe parent
     * @private
     */
    @Watch("dialogD")
    public __dialogD() {
      if (!this.dialogD) {
        this.$emit("closed-Dialog", this.dialogD);
      }
    }

    @Watch("interpret.Name", { immediate: true, deep: true })
    public __interpretName() {
      if (this.firstValidate) {
        if (this.interpret.Name.length > 0) {
          this.errorsName = [];
        }
      }
    }

    @Watch("interpretProp", { immediate: true, deep: true })
    public __setMusicFile() {
      this.interpret = {...this.interpretProp};
      this.errorsName = [];
    }
  }
</script>

<style scoped></style>
