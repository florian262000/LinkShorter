export default interface UserShortlink {
  id: string;
  shortPath: string;
  targetUrl: string;
  clickCounter: number;
  timeStamp: string;
  creatorId: string;
}
