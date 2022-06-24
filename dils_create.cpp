Rp66File* pdlis_File = new Rp66File();
int result = 0;
{
 // 写DLIS 文件 演示代码
				result = pdlis_File->Open("22.dlis", Rp66File::AccessType::Write, "R66LIB-CLIENT", 1, 0, 0);
				if (result == -1)
					assert(false);
				Rp66Set* OriginSet = pdlis_File->CreateOriginSet();
				unsigned long Origin = 0, Copy = 0;
				OriginSet->GetOrigin(Origin);
				
				Rp66Object* Object = OriginSet->CreateObject("DLIS_DEFINING_ORIGIN", Origin, Copy);
				Object->SetAttributeValue("FILE-ID", "Rp66LibClient");
				Object->SetAttributeValue("FILE-SET-NAME", "SHELL/UD550_8");
				int AttrValue = 41;
				Object->SetAttributeValue("FILE-SET-NUMBER", &AttrValue, 1);
				AttrValue = 5;
				Object->SetAttributeValue("FILE-NUMBER", &AttrValue, 1);
				Object->SetAttributeValue("FILE-TYPE", "SEIS");
				Object->SetAttributeValue("PRODUCT", "Horizon");
				Object->SetAttributeValue("VERSION", "8C0-609");
				StringVector strAttrValue;
				strAttrValue.push_back("CSAT1: Combined Seismic Acquisition Tool 1");
				strAttrValue.push_back("CSAT1: Combined Seismic Acquisition Tool 2");
				strAttrValue.push_back("CSAT1: Combined Seismic Acquisition Tool 3");
				Object->SetAttributeValue("PROGRAMS", strAttrValue);
				Rp66_Date_t date_;
				date_.date_year= 2022;
				date_.date_month = 6;
				date_.date_day = 18;
				date_.date_hour = 8;
				date_.date_minute = 55;
				date_.date_second = 0;
				date_.date_millisecond = 15;
				date_.date_timezone = 1;
				Object->SetAttributeValue("CREATION-TIME", (struct Rp66_Date_t*)&date_, 1);
				Object->SetAttributeValue("ORDER-NUMBER", "IDL-970301");
				int run_number = 3;
				Object->SetAttributeValue("RUN-NUMBER", &run_number, 1);
				Object->SetAttributeValue("WELL-NAME", "UD-550  [ICB02]");
				Object->SetAttributeValue("FIELD-NAME", "URDANETA WEST");
				int producer_code = 440;
				Object->SetAttributeValue("PRODUCER-CODE", &producer_code, 1);
				Object->SetAttributeValue("PRODUCER-NAME", "Schlumberger");
				Object->SetAttributeValue("COMPANY", "SHELL VENEZUELA S.A.");
				Object->SetAttributeValue("NAME-SPACE-NAME", "SLB");
				
				Rp66Set* p_Set = pdlis_File->Rp66File::CreateSet("EQUIPMENT", "50", Origin);
				p_Set->CreateAttribute("LENGTH", Rp66_Float, 1,"in", 1);
				p_Set->CreateAttribute("TRADEMARK-NAME", Rp66_Ascii,1);
				p_Set->CreateAttribute("PRESSURE", Rp66_Double, 1, "psi", 1);
				p_Set->CreateAttribute("STATUS", Rp66_SInt, 1);
				p_Set->CreateAttribute("SERIAL-NUMBER", Rp66_Double, 1);
				p_Set->CreateAttribute("TEMPERATURE", Rp66_Float, 1, "degF", 1);
				p_Set->CreateAttribute("VOLUME", Rp66_Double, 1, "ft3",1);
				p_Set->CreateAttribute("MINIMUM-DIAMETER", Rp66_Double, 1, "in");
				p_Set->CreateAttribute("WEIGHT", Rp66_Double, 1, "lbm");
				Rp66Object* p_Object = (Rp66Object*)p_Set->CreateObject("CSAT/CSAH1/HOUSING", Origin);
				float len_AttrValue[2];
				len_AttrValue[0] = 75.980003;
				len_AttrValue[1] = 76.989998;
				p_Object->SetAttributeValue("LENGTH", len_AttrValue, 2);
				p_Object->SetAttributeValue("TRADEMARK-NAME", "CSAH-A", 1);
				double pressure = 20000.0;
				p_Object->SetAttributeValue("PRESSURE", &pressure, 1);
				short status = 1;
				p_Object->SetAttributeValue("STATUS", &status, 1);
				double  serial_number = 0.0;
				p_Object->SetAttributeValue("SERIAL-NUMBER", &serial_number, 1);
				float temperature = 350.0;
				p_Object->SetAttributeValue("TEMPERATURE", &temperature, 1);
				double volume = 0.227;
				p_Object->SetAttributeValue("VOLUME", &volume, 1);
				double  m_diameter = 3.625;
				p_Object->SetAttributeValue("MINIMUM-DIAMETER", &m_diameter, 1);
				double weight = 47.36;
				p_Object->SetAttributeValue("WEIGHT", &weight, 1);

				p_Object = (Rp66Object*)p_Set->CreateObject("CSAT/CSAC1/CARTRIDGE", Origin);
				float length = 0.0;
				p_Object->SetAttributeValue("LENGTH", &length, 1);
				p_Object->SetAttributeValue("TRADEMARK-NAME", "CSAC-A");
				pressure = 20000.0;
				p_Object->SetAttributeValue("PRESSURE", &pressure, 1);
				status = 1;
				p_Object->SetAttributeValue("STATUS", &status, 1);
				serial_number = 0.0;
				p_Object->SetAttributeValue("SERIAL-NUMBER", &serial_number, 1);
				temperature = 350.0;
				p_Object->SetAttributeValue("TEMPERATURE", &temperature, 1);
				volume = 0.0;
				p_Object->SetAttributeValue("VOLUME", &volume, 1);
				m_diameter = 0.0;
				p_Object->SetAttributeValue("MINIMUM-DIAMETER", &m_diameter, 1);
				weight = 24.23;
				p_Object->SetAttributeValue("WEIGHT", &weight, 1);

				p_Set->CreateAttribute("NEWLENGTH", Rp66_Float, 1, "in");
				p_Object = (Rp66Object*)p_Set->CreateObject("CSAT/CSASA1/SONDE", Origin, 0);
				length = 109.06;
				p_Object->SetAttributeValue("LENGTH", &length, 1);
				p_Object->SetAttributeValue("TRADEMARK-NAME", "CSAS-A", 1);
				double pressures[3];
				pressures[0] = 20000.0;
				pressures[1] = 30000.0;
				pressures[2] = 40000.0;
				p_Object->SetAttributeValue("PRESSURE", pressures, 3);
				status = 1;
				p_Object->SetAttributeValue("STATUS", &status, 1);
				serial_number = 0.0;
				p_Object->SetAttributeValue("SERIAL-NUMBER", &serial_number, 1);
				temperature = 350.0;
				p_Object->SetAttributeValue("TEMPERATURE", &temperature, 1);
				volume = 0.3966;
				p_Object->SetAttributeValue("VOLUME", &volume, 1);
				m_diameter = 4.0;
				p_Object->SetAttributeValue("MINIMUM-DIAMETER", &m_diameter, 1);
				weight = 154.18;
				p_Object->SetAttributeValue("WEIGHT", &weight, 1);

				p_Set = pdlis_File->CreateSet("PARAMETER","58", Origin);
				p_Set->CreateAttribute("VALUES", Rp66_Ascii, 1);
				p_Set->CreateAttribute("AXIS", Rp66_Ascii, 1);
				p_Set->CreateAttribute("DIMENSION", Rp66_SInt, 1);
				p_Set->CreateAttribute("LONG-NAME", Rp66_Ascii, 1);

				p_Object = p_Set->CreateObject("ALTDPCHAN", Origin);
				p_Object->SetAttributeValue("VALUES", "TVDE");
				p_Object->SetAttributeValue("LONG-NAME", "Alternate depth channel name");

				p_Object = p_Set->CreateObject("PBVSADP", Origin);
				p_Object->SetAttributeValue("VALUES", "NO");
				p_Object->SetAttributeValue("LONG-NAME", "Alternate depth channel playback enabled");
	
				p_Object = (Rp66Object*)p_Set->CreateObject("FP-1", Origin);
				Rp66Attribute *p_Attr = p_Object->GetAttributeByName("VALUES");
				p_Attr->SetRepcode(Rp66_UChar);
				char uchar = 'H';
				p_Object->SetAttributeValue("VALUES", &uchar, 1);
				p_Object->SetAttributeValue("LONG-NAME", "Fake Parameter - 1");
	
				p_Object = p_Set->CreateObject("FP-2", Origin);
				double values[3];
				values[0] = 8.300000000000001;
				values[1] = 8.4;
				values[2] = 8.5;
				p_Attr = p_Object->GetAttributeByName("VALUES");
				p_Attr->SetUnits("1/lbm");
				p_Attr->SetRepcode(Rp66_Double);
				p_Object->SetAttributeValue("VALUES", values, 3);
				p_Object->SetAttributeValue("LONG-NAME", "Fake Parameter - 2");

				p_Object = (Rp66Object*)p_Set->CreateObject("FP-3", Origin);
				int j = 0;
				Rp66_ComplexFloat_t_ cf[3];
				Rp66_ComplexFloat_t cfs;
				do
				{
					cf[j].real = (float(j) + 1.5);
					cfs.push_back(cf[j].real);
					cf[j].imaginary = (float(j + 1) + 1.5);
					cfs.push_back(cf[j].imaginary);
					++j;
				} while (j < 3);
				
				
				p_Attr = p_Object->GetAttributeByName("VALUES");
				p_Attr->SetRepcode(Rp66_ComplexFloat);
				p_Object->SetAttributeValue("VALUES", &cfs, 3);
				p_Object->SetAttributeValue("LONG-NAME", "Fake Parameter - 3");

				p_Set = pdlis_File->CreateSet("CHANNEL","59", Origin);
				p_Set->CreateAttribute("LONG-NAME", Rp66_Ascii, 1);
				p_Set->CreateAttribute("PROPERTIES", Rp66_Ascii, 1);
				p_Set->CreateAttribute("REPRESENTATION-CODE", Rp66_SInt, 1);
				p_Set->CreateAttribute("UNITS", Rp66_Ascii, 1);
				p_Set->CreateAttribute("DIMENSION", Rp66_SInt, 1);
				p_Set->CreateAttribute("AXIS", Rp66_Obname, 1);
				p_Set->CreateAttribute("ELEMENT-LIMIT", Rp66_SInt, 1);
				p_Set->CreateAttribute("SOURCE", Rp66_Objref, 1);

				Rp66Object* p_Obj_Tdep = (Rp66Object*)p_Set->CreateObject("TDEP", Origin);
				p_Obj_Tdep->SetAttributeValue("LONG-NAME", "6-Inch Frame Depth");
				Rp66Repcode_t  code = Rp66_Float;
				p_Obj_Tdep->SetAttributeValue("REPRESENTATION-CODE", (enum Rp66Repcode_t*)&code, 1);
				p_Obj_Tdep->SetAttributeValue("UNITS", "0.1 in");
				int attr = 1;
				p_Obj_Tdep->SetAttributeValue("DIMENSION", &attr, 1);
				attr = 1;
				p_Obj_Tdep->SetAttributeValue("ELEMENT-LIMIT", &attr, 1);

				Rp66Object* p_Obj_Temp = (Rp66Object*)p_Set->CreateObject("TEMP", Origin);
				p_Obj_Temp->SetAttributeValue("LONG-NAME", "6-Inch Frame Time");
				code = Rp66_Double;
				p_Obj_Temp->SetAttributeValue("REPRESENTATION-CODE", (enum Rp66Repcode_t*)&code, 1);
				p_Obj_Temp->SetAttributeValue("UNITS", "ms");
				attr = 1;
				p_Obj_Temp->SetAttributeValue("DIMENSION", &attr, 1);
				attr = 1;
				p_Obj_Temp->SetAttributeValue("ELEMENT-LIMIT", &attr, 1);

				Rp66Object* p_Obj_Bs = (Rp66Object*)p_Set->CreateObject("BS", Origin);
				p_Obj_Bs->SetAttributeValue("LONG-NAME", "Bit Size");
				code = Rp66_UInt;
				p_Obj_Bs->SetAttributeValue("REPRESENTATION-CODE", (enum Rp66Repcode_t*)&code, 1);
				p_Obj_Bs->SetAttributeValue("UNITS", "in");
				attr = 1;
				p_Obj_Bs->SetAttributeValue("DIMENSION", &attr, 1);
				attr = 1;
				p_Obj_Bs->SetAttributeValue("ELEMENT-LIMIT", &attr, 1);

				Rp66Object* p_Obj_chn1 = (Rp66Object*)p_Set->CreateObject("FAKECHANNEL-1", Origin);
				p_Obj_chn1->SetAttributeValue("LONG-NAME", "Fake Channel - 1");
				code = Rp66_UShort;
				p_Obj_chn1->SetAttributeValue("REPRESENTATION-CODE", (enum Rp66Repcode_t*)&code, 1);
				p_Obj_chn1->SetAttributeValue("UNITS", "in");
				attr = 1;
				p_Obj_chn1->SetAttributeValue("DIMENSION", &attr, 1);
				attr = 1;
				p_Obj_chn1->SetAttributeValue("ELEMENT-LIMIT", &attr, 1);

				Rp66Object* p_Obj_chn2 = (Rp66Object*)p_Set->CreateObject("FAKECHANNEL-2", Origin);
				p_Obj_chn2->SetAttributeValue("LONG-NAME", "Fake Channel - 2");
				code = Rp66_ComplexFloat;
				p_Obj_chn2->SetAttributeValue("REPRESENTATION-CODE", (enum Rp66Repcode_t*)&code, 1);
				p_Obj_chn2->SetAttributeValue("UNITS", "in");
				attr = 1;
				p_Obj_chn2->SetAttributeValue("DIMENSION", &attr, 1);
				attr = 1;
				p_Obj_chn2->SetAttributeValue("ELEMENT-LIMIT", &attr, 1);

				p_Set = pdlis_File->CreateSet("FRAME", "60", Origin);
				p_Set->CreateAttribute("DESCRIPTION", Rp66_Ascii, 1);
				p_Set->CreateAttribute("CHANNELS", Rp66_Obname, 1);
				p_Set->CreateAttribute("INDEX-TYPE", Rp66_Ascii, 1);
				p_Set->CreateAttribute("DIRECTION", Rp66_Ascii, 1);
				p_Set->CreateAttribute("SPACING", Rp66_Float, 1, "0.1 in");
				p_Set->CreateAttribute("ENCRYPTED", Rp66_SInt, 1);
				p_Set->CreateAttribute("INDEX-MIN", Rp66_Float, 1);
				p_Set->CreateAttribute("INDEX-MAX", Rp66_Float, 1);
				Rp66_Obname_t obnames[5];
				p_Obj_Tdep->GetOrigin(obnames[0].Origin);
				p_Obj_Tdep->GetCopy(obnames[0].Copy);
				p_Obj_Tdep->GetName(obnames[0].Name);

				p_Obj_Temp->GetOrigin(obnames[1].Origin);
				p_Obj_Temp->GetCopy(obnames[1].Copy);
				p_Obj_Temp->GetName(obnames[1].Name);

				p_Obj_Bs->GetOrigin(obnames[2].Origin);
				p_Obj_Bs->GetCopy(obnames[2].Copy);
				p_Obj_Bs->GetName(obnames[2].Name);

				p_Obj_chn1->GetOrigin(obnames[3].Origin);
				p_Obj_chn1->GetCopy(obnames[3].Copy);
				p_Obj_chn1->GetName(obnames[3].Name);

				p_Obj_chn2->GetOrigin(obnames[4].Origin);
				p_Obj_chn2->GetCopy(obnames[4].Copy);
				p_Obj_chn2->GetName(obnames[4].Name);

				Rp66Object* p_Obj1s = (Rp66Object*)p_Set->CreateObject("1S", Origin);
				p_Obj1s->SetAttributeValue("CHANNELS", (struct Rp66_Obname_t*)obnames, 5);
				p_Obj1s->SetAttributeValue("INDEX-TYPE", "BOREHOLE-DEPTH");
				float spacing = 1.0;
				p_Obj1s->SetAttributeValue("SPACING", &spacing, 1);
				int encrypted = 1;
				p_Obj1s->SetAttributeValue("ENCRYPTED", &encrypted, 1);
				float index = 0.0;
				p_Obj1s->SetAttributeValue("INDEX-MIN", &index, 1);
				index = 0.0;
				p_Obj1s->SetAttributeValue("INDEX-MAX", &index, 1);


				Rp66Object* p_Obj2s = (Rp66Object*)p_Set->CreateObject("2S", Origin);
				p_Obj2s->SetAttributeValue("CHANNELS", (struct Rp66_Obname_t*)obnames, 5);
				p_Obj2s->SetAttributeValue("INDEX-TYPE", "BOREHOLE-DEPTH");
				spacing = 1.0;
				p_Obj2s->SetAttributeValue("SPACING", &spacing, 1);
				encrypted = 1;
				p_Obj2s->SetAttributeValue("ENCRYPTED", &encrypted, 1);
				index = 0.0;
				p_Obj2s->SetAttributeValue("INDEX-MIN", &index, 1);
				index = 0.0;
				p_Obj2s->SetAttributeValue("INDEX-MAX", &index, 1);

				Rp66Object* p_Obj3s = (Rp66Object*)p_Set->CreateObject("3S", Origin);
				p_Object->SetAttributeValue("CHANNELS", (struct Rp66_Obname_t*)obnames, 5);
				p_Object->SetAttributeValue("INDEX-TYPE", "BOREHOLE-DEPTH");
				spacing = 1.0;
				p_Object->SetAttributeValue("SPACING", &spacing, 1);
				encrypted = 1;
				p_Object->SetAttributeValue("ENCRYPTED", &encrypted, 1);
				index = 0.0;
				p_Object->SetAttributeValue("INDEX-MIN", &index, 1);
				index = 0.0;
				p_Object->SetAttributeValue("INDEX-MAX", &index, 1);

				ParamVector AllParams;// = new ParamVector[4];
				ChannelVector ChannelObjects;// = new ChannelVector[4];
				FrameVector FrameObjects;// = new FrameVector[4];
	
				pdlis_File->ReadAllParams(AllParams);
				pdlis_File->ReadChannelObjects(ChannelObjects);
				pdlis_File->ReadFrameObjects(FrameObjects);
				pdlis_File->WriteStaticData();

				int bufSize;
				int f_size = FrameObjects.size();

				for (int i = 0; i < f_size; ++i)
				{
					FrameObjects[i]->SetupWriteChannels(bufSize, 0);
					char *pBuf = (char*)malloc(bufSize);
					FrameObjects[i]->SetUserBuffer(pBuf);

				}

				for (int i = 0; i < f_size; ++i)
				{
					Rp66Frame *p_Frame = FrameObjects[i];
					for (int j = 0; j < 100; ++j)// 100 frame
					{
						int OffsetInBuffer = ChannelObjects[0]->GetOffsetInBuffer();
						char *UserBuffer = p_Frame->GetUserBuffer();
						int par_size = AllParams.size();
						float val = float(j);
						*(float*)&UserBuffer[OffsetInBuffer] = val;

						for (int k = 1; k < par_size; ++k)
						{
							int chn_offset = ChannelObjects[k]->GetOffsetInBuffer();

							switch (ChannelObjects[k]->GetRepcode())
							{
							case 2:
								*(unsigned short*)&UserBuffer[chn_offset] = 1;
								break;
							case 4:
								*(unsigned int*)&UserBuffer[chn_offset] = j;
								break;
							case 6:
								*(float*)&UserBuffer[chn_offset] = val * val + 0.009999999776482582;
								break;
							case 7:
								*(double*)&UserBuffer[chn_offset] = (double)j * ((double)j * (double)j) + 0.01;
								break;
							case 8:
								*(float*)&UserBuffer[chn_offset] = val;
								*(float*)&UserBuffer[chn_offset + 4] = (double)j + 0.5;
								break;
							default:
								break;
							}
					
						}
						p_Frame->WriteNextFrame();
					}
				}

				for (int i = 0; i < f_size; ++i)
				{
					char *pBuf = FrameObjects[i]->GetUserBuffer();
					if (pBuf)
						free(pBuf);

				}
				pdlis_File->Close();

}
