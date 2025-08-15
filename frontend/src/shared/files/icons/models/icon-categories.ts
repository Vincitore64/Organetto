type IconCategory = 'image'|'video'|'audio'|'pdf'|'archive'|'code'|'sheet'|'slides'|'doc'|'file'

const extByCategory: Record<IconCategory, string[]> = {
  image: ['png','jpg','jpeg','gif','webp','svg','bmp','tiff','heic','avif'],
  video: ['mp4','mov','avi','mkv','webm','m4v'],
  audio: ['mp3','wav','flac','aac','ogg','m4a','oga'],
  pdf:   ['pdf'],
  archive:['zip','rar','7z','tar','gz','bz2','xz'],
  code:  ['js','ts','tsx','jsx','vue','json','xml','yml','yaml','cs','java','py','rb','go','rs','php','sql','html','css','scss','less'],
  sheet: ['xls','xlsx','ods','csv'],
  slides:['ppt','pptx','odp','key'],
  doc:   ['doc','docx','odt','rtf','txt','md'],
  file:  []
}

export {
  type IconCategory,
  extByCategory
}
