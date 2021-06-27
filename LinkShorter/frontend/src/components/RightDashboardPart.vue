<template>
  <div>
    <v-expansion-panels>
      <v-expansion-panel v-bind:key="item" v-for="item in jsonStore">
        <v-expansion-panel-header>
          {{ item["ShortPath"] }} 👉 {{ item["TargetUrl"] }}
        </v-expansion-panel-header>
        <v-expansion-panel-content>
          {{ item["ClickCounter"] }}
        </v-expansion-panel-content>
      </v-expansion-panel>
    </v-expansion-panels>
  </div>
</template>

<script>
//import ShortLinkComponent from "./ShortLinkComponent.vue";
export default {
  //components: { ShortLinkComponent },
  data: () => ({
    jsonStore: null,
  }),
  methods: {
    getLinkData: function () {
      let vm = this;
      this.$http
        .request({
          method: "get",
          url: "https://localhost:5001/api/link/getuserspecificlinks",
        })
        .then(function (response) {
          console.log(response.data);
          vm.jsonStore = response.data;
          console.log(vm.jsonStore);
        })
        .catch(function (error) {
          console.log(error);
        });
    },
  },
  mounted() {
    this.getLinkData();
  },
};
</script>