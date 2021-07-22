import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    isLoggedIn: false,
    userId: -1,
    username: "",
    sessionCookie: "",
  },
  mutations: {
    setIsLoggedIn(state, value) {
      state.isLoggedIn = value;
    },
    setUserId(state, id) {
      state.userId = id;
    },
    setUsername(state, username) {
      state.username = username;
    },
    setSessionCooke(state, sessionCookie) {
      state.sessionCookie = sessionCookie;
    },
  },
  actions: {},
  modules: {},
  getters: {
    getUserId: (state) => {
      return state.userId;
    },
    getIsLoggedIn: (state) => {
      return state.isLoggedIn;
    },
    getUsername: (state) => {
      return state.username;
    },
    getSessionCookie: (state) => {
      return state.sessionCookie;
    },
  },
});
