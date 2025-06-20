export interface GuestHouse {
  id: number;
  name: string;
  address: string;
}

export interface Room {
  id: number;
  name: string;
  guestHouseId: number;
}

export interface Bed {
  id: number;
  bedNumber: string;
  roomId: number;
}