
export interface RoomData { 
  id: number;
  roomNumber: string;
  floor: number; 
  building: string;
  roomType: string;
  description: string;
  capacity: number;
  amenities: string;
  infoPage: string;
}


export const roomData: RoomData[] = [
  // Lecture Hall
    {
      id:1,
      roomNumber: "LH101",
      floor: 1,
      building: "Lecture Hall",
      roomType: "Lecture room",
      description:"character throat suddenly actually phrase tune fun planet save coach lungs zoo football energy easier struggle border practice sides joy deeply available young applied",
      capacity: 50,
      amenities: "[Projector, Whiteboard]",
      infoPage: "/rooms/LH101",
    },
    {
      id:2,
      roomNumber: "LH205",
      floor: 2,
      building: "Lecture Hall",
      roomType: "Office",
      description:"game cent east at sunlight broad basket limited nuts farmer carried difficulty rocket station were research bite calm sense shallow mistake coat education life",
      capacity: 100,
      amenities: "[Projector, Whiteboard, Sound System]",
      infoPage: "/rooms/LH205"
    },
    {
      id:3,
      roomNumber: "LH303",
      floor: 3,
      building: "Lecture Hall",
      roomType: "Lecture room",
      description:"account fire brain still kitchen bring work know noise heard southern job attack sat allow mostly pen although younger color there lake apart evening",
      capacity: 50,
      amenities: "[Projector, Whiteboard]",
      infoPage: "/rooms/LH303"
    },
    {
      id:4,
      roomNumber: "LH404",
      floor: 4,
      building: "Lecture Hall",
      roomType: "Meeting Room",
      description:"addition wall rope needle same replied hunt short wrapped feature however answer hard dug all carbon whole leaf report pot simple did many plant",
      capacity: 100,
      amenities: "[Projector, Whiteboard, Sound System]",
      infoPage: "/rooms/LH404"
    },
    // Academic Building
    {
      id:5,
      roomNumber: "AC1-101",
      floor: 1,
      building: "Academic Cluster 1",
      roomType: "Lecture room",
      description:"gas fair nearer depend night home send indicate wagon yard frame mind famous along make pleasure already require factory course chosen write greater other",
      capacity: 50,
      amenities: "[Projector, Whiteboard]",
      infoPage: "/rooms/AC1-101 "
    },
    {
      id:6,
      roomNumber: "AC1-201",
      floor: 2,
      building: "Academic Cluster 1",
      roomType: "Lecture room",
      description:"noon few give spirit planning want once written bare tax nice invented brush alive consist change whether generally primitive but pack rabbit root flat",
      capacity: 100,
      amenities: "[Projector, Whiteboard, Sound System]",
      infoPage: "/rooms/AC1-201"
    },
    {
      id:7,
      roomNumber: "AC1-301",
      floor: 3,
      building: "Academic Cluster 1",
      roomType: "Meeting Room",
      description:"clock common got story nation does increase wooden mark mix themselves education rather few question fish aid beyond floating model recent constantly check slowly",
      capacity: 100,
      amenities: "[Projector, Whiteboard, Sound System]",
      infoPage: "/rooms/AC1-301"
    },
    {
      id:8,
      roomNumber: "AC1-401",
      floor: 4,
      roomType: "Office",
      building: "Academic Cluster 1",
      description:"sun forget did tide tight split younger age dinner needle stand wire shelf gain cry grade steam positive peace mirror feet secret detail foot",
      capacity: 100,
      amenities: "[Projector, Whiteboard, Sound System]",
      infoPage: "/rooms/AC1-401"
    },
    // Administration Building
    {
      id:9,
      roomNumber: "AB101",
      floor: 1,
      building: "Administration Building",
      roomType: "Meeting Room",
      description:"roof highway tower cannot industry halfway soft sharp exact nails nor spoken consider almost science bare fur prevent next two bill fireplace himself beginning",
      capacity: 50,
      amenities: "[Projector, Whiteboard]",
      infoPage: "/rooms/AB101 "
    },
    {
      id:10,
      roomNumber: "AB201",
      floor: 2,
      building: "Administration Building",
      roomType: "Office",
      description:"rough nothing her when value changing symbol these close tool origin fog decide early serious shade ran advice phrase luck sometime teeth full evidence",
      capacity: 100,
      amenities: "[Projector, Whiteboard, Sound System]",
      infoPage: "/rooms/AB201"
    },
    {
      id:11,
      roomNumber: "AB301",
      floor: 3,
      building: "Administration Building",
      roomType: "Meeting Room",
      description:"us heart search gain voyage ship many event wave willing exercise freedom active prevent thy yet trouble think town lie acres giving tune layers",
      capacity: 100,
      amenities: "[Projector, Whiteboard, Sound System]",
      infoPage: "/rooms/AB301"
    },
    {
      id:12,
      roomNumber: "AB401",
      floor: 4,
      building: "Administration Building",
      roomType: "Conference Room",
      description:"fly title origin either enter yet cause acres easy club least machinery land certainly stuck third means research average wait strip youth silver had",
      capacity: 100,
      amenities: "[Projector, Whiteboard, Sound System]",
      infoPage: "/rooms/AB401"
    },
  ]

  export const buildingData = [
    {
        name: 'Lecture Hall',
    },

    {
        name: 'Administration Building',
    },

    {
        name: 'Academic Cluster 1',
    }
];


