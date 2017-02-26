SetFilterMTMode("DEFAULT_MT_MODE", MT_SERIALIZED)
SetFilterMTMode("LWLibavVideoSource", MT_SERIALIZED)

video="___video___"
audio="___audio___"

v = LWLibavVideoSource(video, cache=true, dr=true, repeat=true, dominance=1).AssumeFPS(30000, 1001)
v = v.height() == 1088 ? v.Crop(0, 0, 0, -8) : v
AudioDub(v, WAVSource(audio))

Prefetch(4)

return last