<template>
  <div v-if="apiKey">
    <h1 class="text-h4">Your API key: {{ apiKey }}</h1>
    <br />
    <p>
      This can be used to generate shortlinks via API calls. <br />
      Calling <code>/api/link/add</code> will generate a new shortlink on your account. You will have to pass JSON in
      the following format: <code>{ targetUrl: "your_base_url", shortPath?: "optional_shortpath", }</code>
    </p>
  </div>
</template>

<script lang="ts">
import axios from "axios";
import Vue from "vue";

export default Vue.extend({
  data() {
    return {
      apiKey: "",
    };
  },
  mounted() {
    this.fetchApiKey();
  },
  methods: {
    async fetchApiKey() {
      try {
        const response = await axios.get("/api/login/getapikey", { withCredentials: true });

        if (response.status !== 200) {
          throw response;
        }

        const data = await response.data;

        this.apiKey = data.apikey;
      } catch (e) {
        console.log(e);
      }
    },
  },
});
</script>

<style scoped></style>
