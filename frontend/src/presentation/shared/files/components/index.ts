import {
  FileOutlined,
  FileTextOutlined,
  DeleteOutlined,
  DownloadOutlined,
  FileImageOutlined,
  VideoCameraOutlined,
  AudioOutlined,
  FilePptOutlined,
  FilePdfOutlined,
  FileZipOutlined,
  FileExcelOutlined,
  CodeOutlined,
} from '@ant-design/icons-vue'

// ---------- Icon components (minimal inline set) ----------
// const IconBase = (d: string) =>
//   defineComponent({
//     name: 'IconBase',
//     setup: () => () =>
//       h(
//         'svg',
//         {
//           viewBox: '0 0 24 24',
//           width: 20,
//           height: 20,
//           fill: 'currentColor',
//           'aria-hidden': 'true',
//         },
//         [h('path', { d })],
//       ),
//   })

const IconFile = FileOutlined // IconBase('M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8zM14 2v6h6')
const IconImage = FileImageOutlined
const IconVideo = VideoCameraOutlined// IconBase('M3 5h12v14H3z M21 7l-6 4v2l6 4z')
const IconAudio = AudioOutlined // IconBase('M12 3v10.55A4 4 0 1 1 10 14V7m2-4h4')
const IconPdf =  FilePdfOutlined
const IconZip = FileZipOutlined // IconBase('M7 2h6l4 4v16H7z M10 2v4m0 2v2m0 2v2m0 2v2')
const IconCode = CodeOutlined // IconBase('M9 18l-6-6 6-6M15 6l6 6-6 6')
const IconSheet = FileExcelOutlined // IconBase('M5 3h14v18H5z M5 7h14M5 11h14M5 15h14')
const IconPpt = FilePptOutlined // IconBase('M4 3h16v18H4z M8 8h5a3 3 0 0 1 0 6H8z')
const IconDoc = FileTextOutlined // IconBase('M6 2h8l4 4v16H6z M8 8h8M8 12h8M8 16h5')
const IconDownload = DownloadOutlined // IconBase('M12 3v12m0 0l-4-4m4 4l4-4M5 21h14')
const IconTrash = DeleteOutlined // IconBase('M3 6h18M8 6V4h8v2m-1 0v12a2 2 0 0 1-2 2H9a2 2 0 0 1-2-2V6')

export {
  IconFile,
  IconImage,
  IconVideo,
  IconAudio,
  IconPdf,
  IconZip,
  IconCode,
  IconSheet,
  IconPpt,
  IconDoc,
  IconDownload,
  IconTrash,
}
